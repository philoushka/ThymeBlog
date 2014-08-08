
namespace Thyme.Tests.JsonSamples
{
    public class GitHubJsonMessages
    {

        public static string GitHubCommitValidMsg
        {
            get
            {
                return @"{
  'ref': 'refs/heads/master',
  'after': 'ea755f71994b3725ac53e8422f9a5937a3466097',
  'before': 'd8ad2ba190b39ff7aef07990b30e63ec6613a347',
  'created': false,
  'deleted': false,
  'forced': false,
  'compare': 'https://github.com/philoushka/blog/compare/d8ad2ba190b3...ea755f71994b',
  'commits': [
    {
      'id': 'ea755f71994b3725ac53e8422f9a5937a3466097',
      'distinct': true,
      'message': 'add js to title.',
      'timestamp': '2014-08-07T09:38:21-07:00',
      'url': 'https://github.com/philoushka/blog/commit/ea755f71994b3725ac53e8422f9a5937a3466097',
      'author': {
        'name': 'Phil Campbell',
        'email': 'philoushka@gmail.com',
        'username': 'philoushka'
      },
      'committer': {
        'name': 'Phil Campbell',
        'email': 'philoushka@gmail.com',
        'username': 'philoushka'
      },
      'added': [

      ],
      'removed': [

      ],
      'modified': [
        'azure-table-storage-library-for-nodejs-frustrations.md'
      ]
    }
  ],
  'head_commit': {
    'id': 'ea755f71994b3725ac53e8422f9a5937a3466097',
    'distinct': true,
    'message': 'add js to title.',
    'timestamp': '2014-08-07T09:38:21-07:00',
    'url': 'https://github.com/philoushka/blog/commit/ea755f71994b3725ac53e8422f9a5937a3466097',
    'author': {
      'name': 'Phil Campbell',
      'email': 'philoushka@gmail.com',
      'username': 'philoushka'
    },
    'committer': {
      'name': 'Phil Campbell',
      'email': 'philoushka@gmail.com',
      'username': 'philoushka'
    },
    'added': [

    ],
    'removed': [

    ],
    'modified': [
      'azure-table-storage-library-for-nodejs-frustrations.md'
    ]
  },
  'repository': {
    'id': 10918224,
    'name': 'blog',
    'full_name': 'philoushka/blog',
    'owner': {
      'name': 'philoushka',
      'email': 'philoushka@gmail.com'
    },
    'private': false,
    'html_url': 'https://github.com/philoushka/blog',
    'description': 'blog',
    'fork': false,
    'url': 'https://github.com/philoushka/blog',
    'forks_url': 'https://api.github.com/repos/philoushka/blog/forks',
    'keys_url': 'https://api.github.com/repos/philoushka/blog/keys{/key_id}',
    'collaborators_url': 'https://api.github.com/repos/philoushka/blog/collaborators{/collaborator}',
    'teams_url': 'https://api.github.com/repos/philoushka/blog/teams',
    'hooks_url': 'https://api.github.com/repos/philoushka/blog/hooks',
    'issue_events_url': 'https://api.github.com/repos/philoushka/blog/issues/events{/number}',
    'events_url': 'https://api.github.com/repos/philoushka/blog/events',
    'assignees_url': 'https://api.github.com/repos/philoushka/blog/assignees{/user}',
    'branches_url': 'https://api.github.com/repos/philoushka/blog/branches{/branch}',
    'tags_url': 'https://api.github.com/repos/philoushka/blog/tags',
    'blobs_url': 'https://api.github.com/repos/philoushka/blog/git/blobs{/sha}',
    'git_tags_url': 'https://api.github.com/repos/philoushka/blog/git/tags{/sha}',
    'git_refs_url': 'https://api.github.com/repos/philoushka/blog/git/refs{/sha}',
    'trees_url': 'https://api.github.com/repos/philoushka/blog/git/trees{/sha}',
    'statuses_url': 'https://api.github.com/repos/philoushka/blog/statuses/{sha}',
    'languages_url': 'https://api.github.com/repos/philoushka/blog/languages',
    'stargazers_url': 'https://api.github.com/repos/philoushka/blog/stargazers',
    'contributors_url': 'https://api.github.com/repos/philoushka/blog/contributors',
    'subscribers_url': 'https://api.github.com/repos/philoushka/blog/subscribers',
    'subscription_url': 'https://api.github.com/repos/philoushka/blog/subscription',
    'commits_url': 'https://api.github.com/repos/philoushka/blog/commits{/sha}',
    'git_commits_url': 'https://api.github.com/repos/philoushka/blog/git/commits{/sha}',
    'comments_url': 'https://api.github.com/repos/philoushka/blog/comments{/number}',
    'issue_comment_url': 'https://api.github.com/repos/philoushka/blog/issues/comments/{number}',
    'contents_url': 'https://api.github.com/repos/philoushka/blog/contents/{+path}',
    'compare_url': 'https://api.github.com/repos/philoushka/blog/compare/{base}...{head}',
    'merges_url': 'https://api.github.com/repos/philoushka/blog/merges',
    'archive_url': 'https://api.github.com/repos/philoushka/blog/{archive_format}{/ref}',
    'downloads_url': 'https://api.github.com/repos/philoushka/blog/downloads',
    'issues_url': 'https://api.github.com/repos/philoushka/blog/issues{/number}',
    'pulls_url': 'https://api.github.com/repos/philoushka/blog/pulls{/number}',
    'milestones_url': 'https://api.github.com/repos/philoushka/blog/milestones{/number}',
    'notifications_url': 'https://api.github.com/repos/philoushka/blog/notifications{?since,all,participating}',
    'labels_url': 'https://api.github.com/repos/philoushka/blog/labels{/name}',
    'releases_url': 'https://api.github.com/repos/philoushka/blog/releases{/id}',
    'created_at': 1372102843,
    'updated_at': '2014-06-18T19:19:45Z',
    'pushed_at': 1407429514,
    'git_url': 'git://github.com/philoushka/blog.git',
    'ssh_url': 'git@github.com:philoushka/blog.git',
    'clone_url': 'https://github.com/philoushka/blog.git',
    'svn_url': 'https://github.com/philoushka/blog',
    'homepage': null,
    'size': 10952,
    'stargazers_count': 0,
    'watchers_count': 0,
    'language': 'JavaScript',
    'has_issues': true,
    'has_downloads': true,
    'has_wiki': true,
    'forks_count': 0,
    'mirror_url': null,
    'open_issues_count': 0,
    'forks': 0,
    'open_issues': 0,
    'watchers': 0,
    'default_branch': 'master',
    'stargazers': 0,
    'master_branch': 'master'
  },
  'pusher': {
    'name': 'philoushka',
    'email': 'philoushka@gmail.com'
  }
}";
            }
        }
    }
}
