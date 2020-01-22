# ペンギン落とし Game

A game developed with Unity

## Developement environment

Unity 2019.2.11f1

Android Studio

XCode 11.3

Cocoapods 1.8.4

Gems 2.6.0

MacOS 10.15.2

## How to open project

### Install Homebrew
```bash
/usr/bin/ruby -e "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install)"
```

### Install git-lfs
```bash
brew install git-lfs
```

### Clone project
```bash
git clone https://github.com/liongarden/penguin-casual-app.git
```
### Download Unity
1. Download [Unity Hub](https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.dmg)

2. Activate license

3. Download [Unity 2019.2.11f1](unityhub://2019.2.11f1/5f859a4cfee5) using Unity Hub

### Open Project
Use Unity 2019.2.11f1 to open cloned Unity project


## How to build project

### Android
Ensure that JDK, NDK, Gradle and Android SDK path is correct in Unity -> Preferences

Cmd+Shift+B to open Build Settings, select Android. Then tick on Export Project and click on Export button.

Build Android project using Android studio or by command:

```bash
gradlew assemble
```

### IOS
Ensure that Cocoapods is installed and put into path with bash
Cmd+Shift+B to open Build Settings, select Android. Then click on Build.

Build IOS project with XCode using the generated xcodeworkspace.

## Libraries using in project

[DOTween 1.2.305](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)

[Firebase SDK 6.9.0](https://firebase.google.com/download/unity)

[GoogleMobileAds 4.2.1](https://github.com/googleads/googleads-mobile-plugins/releases/latest)

[TagActionText](https://assetstore.unity.com/packages/tools/gui/tag-action-text-136444)

[Spine](http://esotericsoftware.com/spine-unity-download)

[Mobile Native Dialog](https://github.com/PingAK9/Mobile-Dialog-Unity)
