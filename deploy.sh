UNITYEDITOR=/e/Unity/Editor/Unity.exe

WD=`pwd`
DIST=Play

mkdir -p $WD/$DIST

$UNITYEDITOR -quit -batchMode -nographics -projectPath "$WD/Breakout-Clone" -buildTarget web -buildWebPlayer "$WD/$DIST"

git add $DIST
git commit


git subtree push --prefix $DIST origin gh-pages
