# Android Barcode Scanner

__Disclaimer: this is just a proof of concept project. In no way or form it could be used as is in production.__

## Introduction
Since Google ML Kit allowed for offline usage of pregenerated neural networks I got interested in seeing if I can make a simple barcode reading application. For that, I needed a wrapper to abstract ML Kit interface and provide an easy to use API to application.
This is an application part implemented in Unity. Only due to me not knowing Android Studio and having zero need to study it.
This prototype failed to meet requirement of rendering editable excel-style table.

## Build
Library built with "Android Barcode Scanner Library" should be put into Assets/Plugins/Android/app-*.aar.
Assets\StreamingAssets\ps2_db.json contains a sample of barcode database.

## Usage
On startup press "Initialize DB" once to populate DB data. After that use "Camera" to look for barcode.

## Dependencies
Newtonsoft.Json
libsqlite3
com.unity.uiwidgets