using edumis.Models.SMC;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace edumisbackend.Helpers;

public static class SmcAppNotifier
{
    private static bool _initialized = false;

    private static void InitializeFirebase()
    {
        if (_initialized) return;
        var app = FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.GetApplicationDefault()
        });
        _initialized = true;
    }

    public static async Task<string?> SendNotificationAsync(string topic, string title, string body)
    {
        InitializeFirebase();
        var message = new Message()
        {
            Topic = topic,
            Notification = new Notification()
            {
                Title = title,
                Body = body
            }
        };
        string? response = null;
        try
        {
            response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
        catch (Exception e)
        {
            // ignored
        }

        return response; 
    }

    public static void SendNotificationSilently(string topic, string title, string body)
    {
        
        _ = Task.Run(async () => {
            try {
                InitializeFirebase();
                await FirebaseMessaging.DefaultInstance.SendAsync(new Message {
                    Topic = topic,
                    Notification = new Notification() {
                        Title = title,
                        Body = body
                    }
                });
            }
            catch (Exception ex)
            {
                // ignored
            }
        });
    }
    
    public static void SendNotificationSilently(Message message) {
        
        _ = Task.Run(async () => {
            try {
                InitializeFirebase();
                await FirebaseMessaging.DefaultInstance.SendAsync(message);
            }
            catch (Exception ex)
            {
                // ignored
            }
        });
    }
    
}
