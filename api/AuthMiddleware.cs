using System;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// ...

public class AuthMiddleware
{
  public static string serviceAccountString = "ewogICJ0eXBlIjogInNlcnZpY2VfYWNjb3VudCIsCiAgInByb2plY3RfaWQiOiAiZG0tYWktZjUzNzAiLAogICJwcml2YXRlX2tleV9pZCI6ICIzMmNkMGE0OWM1Yzg1OWZmMzBkNjlmNjllM2ZlYTRiZDU5ODEwZjY0IiwKICAicHJpdmF0ZV9rZXkiOiAiLS0tLS1CRUdJTiBQUklWQVRFIEtFWS0tLS0tXG5NSUlFdlFJQkFEQU5CZ2txaGtpRzl3MEJBUUVGQUFTQ0JLY3dnZ1NqQWdFQUFvSUJBUURzbHJEa2NyUWRlMVA3XG5IVUswV3lVV1RIOWlaTmo5eU0zSGtoYmpDWldnMmU3d3V3NWI3WCttbU1hdUMvMHZidDV3aU9HM1FkOWVsZDR6XG43NExoSXBpTy9wdmZ0VWdhTUwzV3lCMXZMUzBGU2sxOHhpc0lXeHdtdzRqZktPdEE5cnh3S2NlUi81TURsdlgvXG5GcEJ4OFFOdnliZmpOYkUzMFZuMWJPa0w2YnhJVlhUdHBySFdxQWc3bFc0cUhHdkM1TUVOeGM0S1FGNEJMMzZWXG5xVTk1MWJuSDJOaVd4SUhZMUtmNUVVeG15bVRubXN6TDBBbUVZQ2lUU1VJTWUrTCt2OUczSEJwZVlkQmh3OGJ5XG5IbUxkaXdxa3JoVUZycVhxR3p1TVNiV1hXZVVTKzR2cXZVa2VKaE1vdk1nMlAxdjRVbDVEYXhzSWJHdStSRzFLXG5TVzNLbzFybEFnTUJBQUVDZ2dFQWMwcHh0RG1qUlJTaTFhV3FLbHFhSlpZc28wV3lxbVZ1eGZpT2Q1L2xqd2QwXG5sVklLU3NqZklrK2p6Zy9nbHZ0SG9YNUt4RE54QnRZd2ZJMlVjV0tiQmwvVGxMM3gvM1ZPSlhyS1FURzZGVEJpXG50MUdZaWM5Z1ZoR0dRdjFkT3dDaHptVEZzSU5qWUdYeHhaZDQxMVdVRDZjMG1aL0NGd25KZFR4WUNxcWh5bnNPXG5oMis4NFhjN0ZYOEl6dzdCMXQ3cXI1VThIcDNhcElqUDc4ZGdkczB6MVZRMW1VRUhLMnBFRHVWbXhCaFZtVzJaXG4vVkxZR0pYNWRPNFQ3Q01RNGRRNXlQVVpqamt4cTE5N2VDQUNGVUEyMXpXZkZ6S2hCeGxzUnkxTFpLeWprazNOXG5pbGkrdUpLUmRoeGNobjFxbXBzd2NGbHdRREFzQXo2QUxjVWE4TGV2eHdLQmdRRDU4dCtvWnRoQ3M3ZDVGRm5rXG5wbFZKbVY3elFmVTJpSml0UlJITGszdkZNTmU4Z05RekNOL3NvZ0xqK1lNcTFwUDdTNnJqWkZYNVM1ZWFISHhlXG5Kb1lZblhrTmNuNzh3cUc3K3JNYUFCTzl1M2VoVWJwY21sNk03NFZpd1pabUZWT1lndEg0cVQ0cS9rYVRwMmJKXG5zaWFEYVJUYUFENnY1NVpxdVFINzNPYk1Md0tCZ1FEeVVRTzA2MTgxTk42UFkvVmhxeEcwWVMwZlYvaU1WZ3A2XG51T1Yxd0JQKzJwNUFCNEZLeS93UUE4bVE1aktITjY2R0dIVHBidXQwa3diMVhZd3ZsNkRnKzFpQnBvVzhHamRNXG5YQUM3UlNJamdtckI0aThwVVhMbWtRMFNSV20zUjJ4bTRjTkFEOFJSN0J1QURwZUpudW0rUzhBL1lDcDJMbGg0XG5DZGNOWlFNaEt3S0JnRGkzUXNiOHB6L2pkZE4wcWIwM2ZRelpUM0ZWV3lZSHN0VkdZZlpXdmZRTjFEWkM2V08zXG5OTkNHSnEvQ1UyQXFGcVFrRkYvS3liTnEwcmkybEFYdEtlcDErUnp6Q1J1anNuMXNNcTNJckxJVjB0eDVKaGVUXG5NN2M1Tm9RbE4xSnNybTVoNlBGS3ZmK1ZlVUJSOFFIOWViM0IzMmhrTzlWQWNLSkEzdEZlMjhRakFvR0JBTDNVXG45Z0J3UElBa1VROG9rZjNYMU9EcEdENXIzbzJpZ2tjdVBxVUd1eU4zQlc3SDhtUTBkZDNkK2JVSWdpRW9ZQk14XG5hYlhPYmFzLzI3MnhjYmQvSkV2YzNMT0ZUMVBUZmVyV1VNUmxIcCtPOWkrNkVKUmYrSkhrcE1iaGxqWTlRQkZCXG5ZSUw1VnlXT3dPU0xpZkFJakxuR0FuQWoyR0FKWXNsM2ZBQkVXRFZOQW9HQVF3UHJOS3RSVVNWSGROdlM3andyXG5qU2UxTUtFR3NrV2ZuVjhhSmVEZkpPSjNTRGtEYWtNUzNxMGpOV3pkd2J6aTc3RW5TU1ByaHlNeWd0T2VWRDZwXG5rVUhiQms5WDc2NjJVV3N2VjhmRUxKRmt2Z3NGaWhvb0NtaWFSL3l2MzVMYWxjU1JwTXpRaUw3N0NTNUovZHI1XG5rdUpMdE1YUFhmSVhSazVJa1ZlK2pzcz1cbi0tLS0tRU5EIFBSSVZBVEUgS0VZLS0tLS1cbiIsCiAgImNsaWVudF9lbWFpbCI6ICJmaXJlYmFzZS1hZG1pbnNkay12ZHBhd0BkbS1haS1mNTM3MC5pYW0uZ3NlcnZpY2VhY2NvdW50LmNvbSIsCiAgImNsaWVudF9pZCI6ICIxMTA5Mjg3ODQ3ODYzNTMzMzYxMjkiLAogICJhdXRoX3VyaSI6ICJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20vby9vYXV0aDIvYXV0aCIsCiAgInRva2VuX3VyaSI6ICJodHRwczovL29hdXRoMi5nb29nbGVhcGlzLmNvbS90b2tlbiIsCiAgImF1dGhfcHJvdmlkZXJfeDUwOV9jZXJ0X3VybCI6ICJodHRwczovL3d3dy5nb29nbGVhcGlzLmNvbS9vYXV0aDIvdjEvY2VydHMiLAogICJjbGllbnRfeDUwOV9jZXJ0X3VybCI6ICJodHRwczovL3d3dy5nb29nbGVhcGlzLmNvbS9yb2JvdC92MS9tZXRhZGF0YS94NTA5L2ZpcmViYXNlLWFkbWluc2RrLXZkcGF3JTQwZG0tYWktZjUzNzAuaWFtLmdzZXJ2aWNlYWNjb3VudC5jb20iCn0=";
  public static GoogleCredential credential
  {
    get
    {
      if (_credential == null)
      {
        byte[] data = Convert.FromBase64String(serviceAccountString);
        string jsonKey = System.Text.Encoding.UTF8.GetString(data);
        _credential = GoogleCredential.FromJson(jsonKey);
      }
      return _credential;
    }
  }
  private static GoogleCredential _credential;


  public static FirebaseApp firebaseApp
  {
    get
    {
      if (_firebaseApp == null)
      {
        _firebaseApp = FirebaseApp.Create(new AppOptions { Credential = credential });
      }
      return _firebaseApp;
    }
  }
  private static FirebaseApp _firebaseApp;



  public static async Task<IActionResult> Authenticate(HttpRequest req)
  {
    try
    {
      var idToken = req.Headers["Authorization"].ToString().Replace("Bearer ", "");
      var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;
      var decodedToken = await auth.VerifyIdTokenAsync(idToken);
      return null;
    }
    catch (Exception ex)
    {
      return new UnauthorizedResult();
    }
  }


}
