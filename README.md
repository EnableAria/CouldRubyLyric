# CouldRubyLyric

## 简介

根据网易云歌曲ID生成含平假名注音的word歌词文档。

运行需要 **.Net 8.0 Desktop Runtime**

访问 [Releases](https://github.com/EnableAria/CouldRubyLyric/releases) 以获得最新发行版。

## 程序界面

![](/img/main.png "主程序")

## 运行效果

![](/img/example.png "注音歌词文件示例")

## 特点

1. 将注音罗马字解析为平假名
2. 生成可供修改的Word文档格式

## 不足

1. 由于获取的罗马字并没有标准的规范，在遇到相连的汉字时无法区分
2. 由于获取的罗马字大体根据读音标注，在部分情况下可能解析错误(例如はha被标注为wa)