# MoreBats

[![Build and Release](https://github.com/denpadokei/MoreBats/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/denpadokei/MoreBats/actions/workflows/build-and-release.yml)

セイバーからコウモリのパーティクルを大量に出す MOD です。ゲーム内の Halloween 環境からコウモリのパーティクルを読み込み、セイバーのネオンに取り付けて再生します。

## 機能

- `HalloweenEnvironment` シーンを Addressables から追加読み込みし、`Bats` という名前のパーティクルを取得する
- 取得したパーティクルを複製し、`SaberNeon` の親オブジェクト配下に配置してセイバーに追従させる
- パーティクルの発生量 (`rateOverTime`) を設定値 `BatsCount` に設定する
- 大量発生できるよう `maxParticles` を `int.MaxValue` に設定する
- 元のパーティクルのテクスチャをコピーして専用マテリアルを作り、複製したパーティクルに適用する
- 読み込んだシーンはアンロードし、終了時に生成したオブジェクトとマテリアルを破棄する

## 動作環境

| 項目 | 内容 |
| --- | --- |
| Beat Saber | 1.39.1 |
| 依存 MOD | BSIPA ^4.1.5 / SiraUtil ^3.1.14 / BeatSaberMarkupLanguage ^1.12.3 |

## インストール

1. Beat Saber を 1.39.1 に合わせる (BSManager などのバージョン管理ツールを推奨)
2. BSIPA と上記の依存 MOD をインストールする
3. [Releases](https://github.com/denpadokei/MoreBats/releases) から `MoreBats-2.0.0-bs1.39.1.zip` をダウンロードし、`Plugins` フォルダをゲームフォルダへ上書きコピーする

## 使い方

インストール後は自動で有効になります。メニューに入るとパーティクルの読み込みが行われ、以降はセイバーの周囲にコウモリが表示されます。パーティクル数が多すぎて重い場合は、設定で発生量を下げてください。

## 設定

ゲーム内の `設定` から `More Bats` を開きます。

| 項目 | 型 | 既定値 | 説明 |
| --- | --- | --- | --- |
| BatsCount | int | 100 | パーティクルの発生量 (毎秒)。設定画面のスライダーは 5〜10000 の範囲で、整数のみ |

設定は `UserData/MoreBats.json` に保存されます。

## ビルド

```powershell
git clone https://github.com/denpadokei/MoreBats.git
cd MoreBats
dotnet build .\MoreBats\MoreBats.csproj -c Release
```

`BeatSaberDir` (または `GameDirectory`) に Beat Saber のインストール先を設定すると、ゲームの参照アセンブリを解決できます。

GitHub Actions の `build-and-release.yml` が push ごとにビルドし、`X.X.X` 形式のタグを push するとリリースを作成します。

## ライセンス

MIT
