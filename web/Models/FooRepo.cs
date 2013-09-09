using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using LibGit2Sharp;
namespace Thyme.Web.Models
{
    public class FooRepo: IDisposable
    {
        public void Dispose() { }
        public void LetUsTryThis()
        {
            using (var repo = new Repository(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RepoClone")))
            {
                var referenceCommit = repo.Head.Tip;

                IEnumerable<KeyValuePair<string, DateTimeOffset>> res = LatestChanges(repo, referenceCommit);

                foreach (KeyValuePair<string, DateTimeOffset> kvp in res)
                {
                    Console.WriteLine(string.Format("{0} - {1}", kvp.Key, kvp.Value));
                }
            }
        }

        private IEnumerable<KeyValuePair<string, DateTimeOffset>> LatestChanges(Repository repo, Commit referenceCommit)
        {
            IDictionary<string, DateTimeOffset> dic = new Dictionary<string, DateTimeOffset>();

            var commitLog = repo.Commits.QueryBy(new CommitFilter { Since = referenceCommit });

            var mostRecent = referenceCommit;

            foreach (Commit current in commitLog)
            {
                IEnumerable<KeyValuePair<string, DateTimeOffset>> res = ExtractAdditionsAndModifications(repo, mostRecent, current);
                AddLatest(dic, res);

                mostRecent = current;
            }

            return dic.OrderByDescending(kvp => kvp.Value);
        }

        private IEnumerable<KeyValuePair<string, DateTimeOffset>> ExtractAdditionsAndModifications(Repository repo, Commit tree1, Commit tree)
        {
            IDictionary<string, DateTimeOffset> dic = new Dictionary<string, DateTimeOffset>();

            var tc = repo.Diff.Compare(tree.Tree, tree1.Tree);

            foreach (TreeEntryChanges treeEntryChanges in tc.Added)
            {
                dic.Add(treeEntryChanges.Path, tree1.Committer.When);
            }

            foreach (TreeEntryChanges treeEntryChanges in tc.Modified)
            {
                dic.Add(treeEntryChanges.Path, tree1.Committer.When);
            }
            if (dic.Any() == false)
            {
              //  dic.Add(null, tree.Committer.When);
            }
            return dic;
        }

        private void AddLatest(IDictionary<string, DateTimeOffset> main, IEnumerable<KeyValuePair<string, DateTimeOffset>> latest)
        {
            foreach (var kvp in latest)
            {
                if (main.ContainsKey(kvp.Key))
                {
                    continue;
                }

                main.Add(kvp);
            }
        }


    }
}