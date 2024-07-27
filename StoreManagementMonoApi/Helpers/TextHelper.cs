namespace store_management_mono_api.Helpers;

public class TextHelper
{
    public static string GetRandomToken(int tokenLength)
    {
        string token = "";
        Random random = new Random();
        for (int i = 0; i < tokenLength; i++)
        {
            token += "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"[random.Next("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".Length)];
        }

        return token;
    }
}