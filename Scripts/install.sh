#! /bin/sh

# Example install script for Unity3D project. See the entire example: https://github.com/JonathanPorta/ci-build

# This link changes from time to time. I haven't found a reliable hosted installer package for doing regular
# installs like this. You will probably need to grab a current link from: http://unity3d.com/get-unity/download/archive
# To see the available packages see the torrent provided by Unity
Changeset=fc1d3344e6ea
version=2017.3.1f1

brew cask install caskroom/versions/java8
export JAVA_HOME=$(/usr/libexec/java_home)

brew cask install android-sdk
export ANDROID_SDK_ROOT="/usr/local/share/android-sdk"

brew cask install android-ndk
export ANDROID_NDK_HOME="/usr/local/share/android-ndk"

echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacEditorInstaller/Unity.pkg: "
curl -o Unity.pkg http://download.unity3d.com/download_unity/${Changeset}/MacEditorInstaller/Unity.pkg
sudo installer -dumplog -package Unity.pkg -target /

#echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacDocumentationInstaller/Documentation.pkg: "
#curl -o Documentation.pkg http://download.unity3d.com/download_unity/${Changeset}/MacDocumentationInstaller/Documentation.pkg
#sudo installer -dumplog -package Documentation.pkg -target /

# invalid ?
#echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacWebPlayerInstaller/WebPlayer.pkg: "
#curl -o WebPlayer.pkg http://download.unity3d.com/download_unity/${Changeset}/MacWebPlayerInstaller/WebPlayer.pkg
#sudo installer -dumplog -package WebPlayer.pkg -target /

echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacStandardAssetsInstaller/StandardAssets.pkg: "
curl -o StandardAssets.pkg http://download.unity3d.com/download_unity/${Changeset}/MacStandardAssetsInstaller/StandardAssets.pkg
sudo installer -dumplog -package StandardAssets.pkg -target /

#echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacExampleProjectInstaller/Examples.pkg: "
#curl -o Examples.pkg http://download.unity3d.com/download_unity/${Changeset}/MacExampleProjectInstaller/Examples.pkg
#sudo installer -dumplog -package Examples.pkg -target /

# invalid because already inside Unity.pkg?
#echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Mac-Support-for-Editor-${version}.pkg: "
#curl -o UnitySetup-Mac-Support-for-Editor-${version}.pkg http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Mac-Support-for-Editor-${version}.pkg
#sudo installer -dumplog -package UnitySetup-Mac-Support-for-Editor-${version}.pkg -target /

echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Android-Support-for-Editor-${version}.pkg: "
curl -o UnitySetup-Android-Support-for-Editor-${version}.pkg http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Android-Support-for-Editor-${version}.pkg
sudo installer -dumplog -package UnitySetup-Android-Support-for-Editor-${version}.pkg -target /

#echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-iOS-Support-for-Editor-${version}.pkg: "
#curl -o UnitySetup-iOS-Support-for-Editor-${version}.pkg http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-iOS-Support-for-Editor-${version}.pkg
#sudo installer -dumplog -package UnitySetup-iOS-Support-for-Editor-${version}.pkg -target /

#echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-AppleTV-Support-for-Editor-${version}.pkg: "
#curl -o UnitySetup-AppleTV-Support-for-Editor-${version}.pkg http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-AppleTV-Support-for-Editor-${version}.pkg
#sudo installer -dumplog -package UnitySetup-AppleTV-Support-for-Editor-${version}.pkg -target /

echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Linux-Support-for-Editor-${version}.pkg: "
curl -o UnitySetup-Linux-Support-for-Editor-${version}.pkg http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Linux-Support-for-Editor-${version}.pkg
sudo installer -dumplog -package UnitySetup-Linux-Support-for-Editor-${version}.pkg -target /

#echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Samsung-TV-Support-for-Editor-${version}.pkg: "
#curl -o UnitySetup-Samsung-TV-Support-for-Editor-${version}.pkg http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Samsung-TV-Support-for-Editor-${version}.pkg
#sudo installer -dumplog -package UnitySetup-Samsung-TV-Support-for-Editor-${version}.pkg -target /

#echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Tizen-Support-for-Editor-${version}.pkg: "
#curl -o UnitySetup-Tizen-Support-for-Editor-${version}.pkg http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Tizen-Support-for-Editor-${version}.pkg
#sudo installer -dumplog -package UnitySetup-Tizen-Support-for-Editor-${version}.pkg -target /

echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-WebGL-Support-for-Editor-${version}.pkg: "
curl -o UnitySetup-WebGL-Support-for-Editor-${version}.pkg http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-WebGL-Support-for-Editor-${version}.pkg
sudo installer -dumplog -package UnitySetup-WebGL-Support-for-Editor-${version}.pkg -target /

echo "Downloading from http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-${version}.pkg: "
curl -o UnitySetup-Windows-Support-for-Editor-${version}.pkg http://download.unity3d.com/download_unity/${Changeset}/MacEditorTargetInstaller/UnitySetup-Windows-Support-for-Editor-${version}.pkg
sudo installer -dumplog -package UnitySetup-Windows-Support-for-Editor-${version}.pkg -target /
