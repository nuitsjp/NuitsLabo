
namespace OpenAiStudy;

// レスポンスボディのデシリアライズ用クラス
public class OpenAiResponse
{
    public Choice[] choices { get; set; }
    public class Choice
    {
        public string text { get; set; }
    }
}