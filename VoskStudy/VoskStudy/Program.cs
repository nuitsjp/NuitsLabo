using var detector = new SpeechDetector(@"..\..\..\vosk-model-small-ja-0.22");
detector.KeywordDetected += (sender, keyword) =>
{
    Console.WriteLine($"検出されたキーワード: {keyword}");
    // ここでイベントに応じた処理を行う
};

detector.StartListening();
Console.WriteLine("リッスン中... 'Start' または 'Stop' と言ってください。");
Console.ReadLine();

detector.StopListening();