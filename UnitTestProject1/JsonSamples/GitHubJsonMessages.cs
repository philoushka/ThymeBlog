
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
  'after': '467149cc8366e58c1038b2a6a5adcde476a2513b',
  'before': 'ebcd458c333be23931d534007e40217e54a256fc',
  'created': false,
  'deleted': false,
  'forced': false,
  'compare': 'https://github.com/philoushka/blog/compare/ebcd458c333b...467149cc8366',
  'commits': [
    {
      'id': '467149cc8366e58c1038b2a6a5adcde476a2513b',
      'distinct': true,
      'message': 'added a thought or 2 at the bottom\n\nadd pics too.',
      'timestamp': '2014-07-16T13:45:12-07:00',
      'url': 'https://github.com/philoushka/blog/commit/467149cc8366e58c1038b2a6a5adcde476a2513b',
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
        'img/overcast/front-screen.png',
        'img/overcast/smart-speed.png'
      ],
      'removed': [

      ],
      'modified': [
        'img/overcast/overcast-my-podcasts.png',
        'overcast-first-thoughts-mini-review.md'
      ]
    }
  ],
  'head_commit': {
    'id': '467149cc8366e58c1038b2a6a5adcde476a2513b',
    'distinct': true,
    'message': 'added a thought or 2 at the bottom\n\nadd pics too.',
    'timestamp': '2014-07-16T13:45:12-07:00',
    'url': 'https://github.com/philoushka/blog/commit/467149cc8366e58c1038b2a6a5adcde476a2513b',
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
      'img/overcast/front-screen.png',
      'img/overcast/smart-speed.png'
    ],
    'removed': [

    ],
    'modified': [
      'img/overcast/overcast-my-podcasts.png',
      'overcast-first-thoughts-mini-review.md'
    ]
  },
  'repository': {
    'id': 10918224,
    'name': 'blog',
    'url': 'https://github.com/philoushka/blog',
    'description': 'blog',
    'watchers': 0,
    'stargazers': 0,
    'forks': 0,
    'fork': false,
    'size': 10108,
    'owner': {
      'name': 'philoushka',
      'email': 'philoushka@gmail.com'
    },
    'private': false,
    'open_issues': 0,
    'has_issues': true,
    'has_downloads': true,
    'has_wiki': true,
    'language': 'JavaScript',
    'created_at': 1372102843,
    'pushed_at': 1405543521,
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
