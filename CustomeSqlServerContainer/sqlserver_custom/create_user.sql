-- 新しいユーザーの作成
CREATE LOGIN newuser WITH PASSWORD = 'NewUserPass!';
GO

-- マスターデータベースを使用
USE master;
GO

-- 新しいユーザーにデータベースの作成権限を付与
GRANT CREATE DATABASE TO newuser;
GO
