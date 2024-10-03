using LibGit2Sharp;
using System;
using System.IO;

// トークンを第一引数から取得
string token = args[0];

// リポジトリとファイルの設定
string repoPath = @"D:\NuitsLabo";
string fileName = "example.txt";
string filePath = Path.Combine(repoPath, fileName);
string fileContent = "Hello, LibGit2Sharp";
string commitMessage = "Add example.txt";
string branchName = "main";

// ファイルを作成し、内容を書き込む
File.WriteAllText(filePath, fileContent);

// リポジトリを開く
using var repo = new Repository(repoPath);

// 1. ファイルの追加（ステージング）
Commands.Stage(repo, fileName);

// 2. コミット
var author = new Signature("Your Name", "your.email@example.com", DateTimeOffset.Now);
var committer = author;
var commit = repo.Commit(commitMessage, author, committer);

// 3. プッシュ（コマンドライン引数から取得したトークンを使用）
var remote = repo.Network.Remotes["origin"];
var options = new PushOptions
{
    CredentialsProvider = (_url, _user, _cred) =>
        new UsernamePasswordCredentials { Username = token, Password = string.Empty }
};

repo.Network.Push(remote, $"refs/heads/{branchName}", options);