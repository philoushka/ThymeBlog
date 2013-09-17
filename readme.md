Thyme Blog
---------------

###What Is This?###
This is a blog engine that reads your git repo full of markdown  files.

###Technologies###
* ASP.NET MVC
* Markdown
* Windows Azure websites (optional)
* [MarkdownDeep](http://www.toptensoftware.com/markdowndeep/features)

###How Do I Use This?###

This project doesn't require you to use Windows Azure. You could install it on any Windows installation as you like. Here's the 40,000 foot overview.

1. Clone this repo into a repo of your own.
1. Modify the project as you like. CSS, etc.
1. Configure the web.config AppSetting to read from a git repo containing markdown files.
1. Configure your Windows Azure Website to pull from your git repo.
1. Your markdown files are now being served as blog posts. 

###Why###
This blog engine assumes that:

* you want to write your blog posts using a local Markdown editor.
* you don't want complicated management built into the website
* you want to use existing security mechanisms (like your Git repo) to handle authentication when creating/editing posts.
* the Git repo is separate from this blog engine project to allow for an easy fork of the blog engine project, and to allow your blog posts to be stored elsewhere or independant of this blog engine. 

 

####Dirty Details On Your Markdown Files####
The blog engine converts your Markdown files to HTML as they're requested by the browser.

The Markdown file is assumed to have a bit of serialized JSON in an HTML comment as the first line in the Markdown file. Here it reads meta data about the blog post.

    <!-- Title:"Your Blog Post Click Bait Headline",
         PublishedOn:"2010-08-20 07:17", 
         Intro:"Some description about your post. A teaser, if you will."-->