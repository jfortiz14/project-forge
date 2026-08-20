using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Forge.DocumentIntake
{
 public enum IntakeState { Queued, Processing, Completed, Failed, DeadLettered }
 public enum IntakeDecision { Accepted, Conflict, NotClaimable, Claimed, Completed, Failed }
 public record IntakeRequest(string IdempotencyKey, string BlobReference, IReadOnlyDictionary<string,string> Metadata)
 {
  public static IntakeRequest Create(string idempotencyKey,string blobReference,IReadOnlyDictionary<string,string> metadata)
  {
   if(string.IsNullOrWhiteSpace(idempotencyKey)) throw new ArgumentException("Idempotency key cannot be blank.",nameof(idempotencyKey));
   if(string.IsNullOrWhiteSpace(blobReference)) throw new ArgumentException("Blob reference cannot be blank.",nameof(blobReference));
   if(metadata==null) throw new ArgumentNullException(nameof(metadata));
   if(!Uri.TryCreate(blobReference,UriKind.Absolute,out var uri)||uri.Scheme!="http"&&uri.Scheme!="https") throw new ArgumentException("Blob reference must be a valid absolute HTTP/HTTPS URI.",nameof(blobReference));
   var normalizedUri=uri.AbsoluteUri; var metadataList=metadata.OrderBy(kvp=>kvp.Key).ThenBy(kvp=>kvp.Value).ToList(); var metadataString=string.Join(";",metadataList.Select(kvp=>$"{kvp.Key}={kvp.Value}"));
   using var sha256=SHA256.Create(); var hashBytes=sha256.ComputeHash(Encoding.UTF8.GetBytes($"{normalizedUri}{metadataString}")); var fingerprint=BitConverter.ToString(hashBytes).Replace("-","").ToLowerInvariant();
   return new IntakeRequest(idempotencyKey,normalizedUri,metadata);
  }
 }
 public record IntakeRecord(string Identifier,string CorrelationId,string ConcurrencyToken,IntakeRequest Request,string? WorkerAttempt,DateTime? LeaseExpiry,IntakeState State)
 {
  public static IntakeRecord CreateQueued(string identifier,string correlationId,string concurrencyToken,IntakeRequest request)
  {
   if(string.IsNullOrWhiteSpace(identifier)) throw new ArgumentException("Identifier cannot be blank.",nameof(identifier)); if(string.IsNullOrWhiteSpace(correlationId)) throw new ArgumentException("Correlation ID cannot be blank.",nameof(correlationId)); if(string.IsNullOrWhiteSpace(concurrencyToken)) throw new ArgumentException("Concurrency token cannot be blank.",nameof(concurrencyToken));
   return new IntakeRecord(identifier,correlationId,concurrencyToken,request,null,null,IntakeState.Queued);
  }
  public bool Matches(IntakeRequest request) => Request.IdempotencyKey==request.IdempotencyKey && Request.BlobReference==request.BlobReference && Request.Metadata.Count==request.Metadata.Count && Request.Metadata.Keys.All(k=>request.Metadata.ContainsKey(k)&&request.Metadata[k]==Request.Metadata[k]);
  public TransitionResult TryClaim(string workerAttemptId,DateTime nowUtc,TimeSpan leaseDuration,string expectedConcurrencyToken,string newConcurrencyToken)
  {
   if(string.IsNullOrWhiteSpace(workerAttemptId)) throw new ArgumentException("Worker attempt ID cannot be blank.",nameof(workerAttemptId)); if(leaseDuration<=TimeSpan.Zero) throw new ArgumentException("Lease duration must be positive.",nameof(leaseDuration)); if(string.IsNullOrWhiteSpace(expectedConcurrencyToken)) throw new ArgumentException("Expected concurrency token cannot be blank.",nameof(expectedConcurrencyToken)); if(string.IsNullOrWhiteSpace(newConcurrencyToken)) throw new ArgumentException("New concurrency token cannot be blank.",nameof(newConcurrencyToken));
   if(State!=IntakeState.Queued&&State!=IntakeState.Processing) return new TransitionResult(IntakeDecision.NotClaimable,null); if(State==IntakeState.Processing&&LeaseExpiry.HasValue&&LeaseExpiry.Value>nowUtc) return new TransitionResult(IntakeDecision.NotClaimable,null); if(ConcurrencyToken!=expectedConcurrencyToken) return new TransitionResult(IntakeDecision.NotClaimable,null);
   return new TransitionResult(IntakeDecision.Claimed,new IntakeRecord(Identifier,CorrelationId,newConcurrencyToken,Request,workerAttemptId,nowUtc.Add(leaseDuration),IntakeState.Processing));
  }
  public TransitionResult TryComplete(string workerAttemptId,DateTime nowUtc,string expectedConcurrencyToken,string newConcurrencyToken)
  {
   if(string.IsNullOrWhiteSpace(workerAttemptId)) throw new ArgumentException("Worker attempt ID cannot be blank.",nameof(workerAttemptId)); if(string.IsNullOrWhiteSpace(expectedConcurrencyToken)) throw new ArgumentException("Expected concurrency token cannot be blank.",nameof(expectedConcurrencyToken)); if(string.IsNullOrWhiteSpace(newConcurrencyToken)) throw new ArgumentException("New concurrency token cannot be blank.",nameof(newConcurrencyToken)); if(State!=IntakeState.Processing||WorkerAttempt!=workerAttemptId||ConcurrencyToken!=expectedConcurrencyToken) return new TransitionResult(IntakeDecision.NotClaimable,null); return new TransitionResult(IntakeDecision.Completed,new IntakeRecord(Identifier,CorrelationId,newConcurrencyToken,Request,null,null,IntakeState.Completed));
  }
  public TransitionResult TryFail(string workerAttemptId,DateTime nowUtc,string expectedConcurrencyToken,string newConcurrencyToken)
  {
   if(string.IsNullOrWhiteSpace(workerAttemptId)) throw new ArgumentException("Worker attempt ID cannot be blank.",nameof(workerAttemptId)); if(string.IsNullOrWhiteSpace(expectedConcurrencyToken)) throw new ArgumentException("Expected concurrency token cannot be blank.",nameof(expectedConcurrencyToken)); if(string.IsNullOrWhiteSpace(newConcurrencyToken)) throw new ArgumentException("New concurrency token cannot be blank.",nameof(newConcurrencyToken)); if(State!=IntakeState.Processing||WorkerAttempt!=workerAttemptId||ConcurrencyToken!=expectedConcurrencyToken) return new TransitionResult(IntakeDecision.NotClaimable,null); return new TransitionResult(IntakeDecision.Failed,new IntakeRecord(Identifier,CorrelationId,newConcurrencyToken,Request,null,null,IntakeState.Failed));
  }
 }
 public record TransitionResult(IntakeDecision Decision,IntakeRecord? NewRecord);
}
