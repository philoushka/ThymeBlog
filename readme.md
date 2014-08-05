Thyme Blog
---------------

### What Is This?

This is a blog engine that reads a Git repo full of your blog posts in Markdown format.

### Technologies Used

* ASP.NET MVC
* Markdown
* Windows Azure websites (optional)
* [MarkdownDeep](http://www.toptensoftware.com/markdowndeep/features)

### How Do I Use This?

This project doesn't require you to use Windows Azure. You could install it on any Windows installation as you like. Here's the 40,000 foot overview.

1. Fork this repo into a repo of your own.
1. Modify the project as you like. CSS, etc.
1. Configure your deployment via web.config. If using Azure, use these [tweaks](#tweaks). 
1. Visit your site. Your markdown files will be pulled down and will then be served as blog posts. 

### Why Would I Use This?

You want something easy. You want:

- to write your blog posts using a local Markdown editor.
- no complicated management built into the website.
- to use existing security mechanisms (like your Git repo) to handle authentication when creating/editing posts.
- your blog content backed up and/or separate from the blog engine project. 
- your blog site to perform quickly. This enging caches blog posts in memory and uses aggressive cache-busting. More at the [caching section](#caching).
 

#### Dirty Details On Your Markdown Files
The blog engine converts your Markdown files to HTML as they're requested by the browser.

The Markdown file is assumed to have a bit of **serialized JSON in an HTML comment** as the first single line in the Markdown file. Here it reads meta data about the blog post. This meta data is used when listing your posts on the website's front page.

    <!-- {Title:"Your Blog Post Headline",
         PublishedOn:"2010-08-20 07:17", 
         Intro:"Some description about your post. A teaser, if you will."}-->
         
**Change the above to be one line!**

    <!-- {Title:"Your Blog Post Headline", PublishedOn:"2010-08-20 07:17", Intro:"Some description about your post. A teaser, if you will."}-->
         
#### Microsoft Azure
Unless you have a Windows machine with IIS at your disposal, you're probably thinking you need some hosting. Just use [Microsoft Azure](http://azure.microsoft.com/en-us/pricing/free-trial//). The feature:price ration is so much better than regular shared hosting.

![](http://i.imgur.com/0mtNlWa.png)

<img src="http://i.imgur.com/bNyHELF.png" style="border:1px solid black"  >
 
<img src="http://i.imgur.com/SohJzlF.png" style="border:1px solid black"  >

<img src="http://i.imgur.com/EKXP4qm.png" style="border:1px solid black"  >


<a name="caching"/>
#### Caching
So the authoritative data store is a Git repo. I use GitHub, but really it could be any Git repo. This web app has 2 levels of caching. So when the app needs to read a single blog post, or list/search all posts, it will go through these steps, in order:

1. [ASP.NET cache](http://msdn.microsoft.com/en-us/library/system.web.caching.cache.aspx) - for the lifetime of the app, it will load BlogPost objects and put them into memory cache that lives in memory. This is `System.Web.HttpContext.Current.Cache`.

2. Files on disk - a copy of all your markdown files are put on disk on app startup. If local directory is empty, the contents of your Git repo will be downloaded and saved to disk.

##### Reloading
- You can force the app to rebuild its cache by visiting `example.com/SyncDiskToCache`

- You can force the app to pull all items from your Git repo by visiting `example.com/ForceRepoRefresh`.

<a name="tweaks"/>
#### Tweaks You Must Make

After you publish to Azure, head over to the Configure tab. There are some [values in web.config](http://i.imgur.com/IAukH6F.jpg) that you probably don't want to inherit. It's easier to override those by entering those values in Azure (as you might in IIS), rather than editing web.config, and recommiting to source control.

![](http://i.imgur.com/jXfSz0o.png)


