csscss-from-ironruby
===============

This sample is a proof of concept to explore how to invoke [`csscss`](https://github.com/zmoazeni/csscss)
from a .NET program using IronRuby.

The three approaches shown are:

 * use of the engine from .NET to load the program. This requires to specify the folders for all the libraries used by `csscss`;
 * as before, but installing the gems locally with `bundler`(http://bundler.io/) and discovering at runtime the search paths to use to load all the libraries. To install the gems, use the `install_gem_with_bundler.bat` batch;
 * invocation of the `csscss` program with dump of its output. This way it's easier to update the programs but requires the ability to install Ruby and the `csscss` gem;
  

This sample is inspired by this [question](http://stackoverflow.com/questions/15932406/can-ironruby-nuget-package-be-installed-then-used-in-a-vanilla-vs2012) on Stack Overflow.