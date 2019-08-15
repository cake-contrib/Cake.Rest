# Cake.Rest

Cake Add-in to ease consumption of REST APIs using [RestSharp](http://restsharp.org/).

[![License](http://img.shields.io/:license-mit-blue.svg)](https://github.com/cake-contrib/Cake.Rest/blob/master/LICENSE)

## Information

| | master branch |
|---|---|
|NuGet|[![NuGet](https://img.shields.io/nuget/v/Cake.Rest.svg)](https://www.nuget.org/packages/Cake.Rest)|
|AppVeyor|[![Build status](https://img.shields.io/appveyor/ci/cakecontrib/cake-rest.svg)](https://ci.appveyor.com/project/cakecontrib/cake-rest)|
|Travis CI|[![Build status](https://api.travis-ci.com/cake-contrib/Cake.Rest.svg?branch=master)](https://travis-ci.com/cake-contrib/Cake.Rest)|
|Code Coverage|[![Code Coverage](https://img.shields.io/coveralls/github/cake-contrib/Cake.Rest.svg?style=flat)](https://coveralls.io/github/cake-contrib/Cake.Rest)|

## Build

To build this library, we use [Cake](https://cakebuild.net/) and [Cake.Recipe](https://github.com/cake-contrib/Cake.Recipe).

On Windows PowerShell, run the bootstrapper through:

```powershell
.\build.ps1
```

or on Linux/macOS:
```
./build.sh
```

## Credits
- João Antunes (Coding Militia) for [his great walkthrough on using Cake on CI/CD to publish a library to NuGet](http://https://blog.codingmilitia.com/2018/07/30/creating-ci-cd-pipeline-dotnet-library-part-02-defining-the-build-with-cake-publish-nuget). Code from example project used under MIT.

- [@phillipsj](https://github.com/phillipsj) for [Cake.Netlify](https://github.com/cake-contrib/Cake.Netlify), some Cake code and some parts of CI/CD configurations come from that repo.

- [The RestSharp Project](http://restsharp.org/), without RestSharp this would not be this easy.
