#! /bin/sh

# Example build script for Unity3D project. See the entire example: https://github.com/JonathanPorta/ci-build

# Change this the name of your project. This will be the name of the final executables as well.
project="stalnias"
DATE=`date +%y.%m.%d`
versionName="a.${DATE}"


echo "Attempting to build $project for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd)/ \
  -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe" \
  -quit

echo 'Logs from build'
cat $(pwd)/unity.log

echo "Attempting to build $project for OS X"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd)/ \
  -buildOSXUniversalPlayer "$(pwd)/Build/osx/$project.app" \
  -quit

echo 'Logs from build'
cat $(pwd)/unity.log

echo "Attempting to build $project for Linux"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd)/ \
  -buildLinuxUniversalPlayer "$(pwd)/Build/linux/$project" \
  -quit

echo 'Logs from build'
cat $(pwd)/unity.log

echo "Attempting to build $project for WebGL"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd)/ \
  -quit \
  -executeMethod BuildScript.BuildWebGL "$(pwd)/Build/webgl/$project/"
echo 'Logs from build'
cat $(pwd)/unity.log

#export ANDROID_SDK_ROOT="/usr/local/share/android-sdk"
#export ANDROID_NDK_HOME="/usr/local/share/android-ndk"
#export JAVA_HOME=$(/usr/libexec/java_home)

#echo "ANDROID_SDK_ROOT="${ANDROID_SDK_ROOT}
#echo "ANDROID_NDK_HOME="${ANDROID_NDK_HOME}
#echo "JAVA_HOME="${JAVA_HOME}

#echo "Attempting to build $project for Android"
#/Applications/Unity/Unity.app/Contents/MacOS/Unity \
#  -batchmode \
#  -nographics \
#  -silent-crashes \
#  -logFile $(pwd)/unity.log \
#  -projectPath $(pwd)/ \
#  -quit \
#  -executeMethod BuildScript.BuildAndroid $(pwd)/Build/android/${project}.${versionName}.apk
#echo 'Logs from build'
#cat $(pwd)/unity.log