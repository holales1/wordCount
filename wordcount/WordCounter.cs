﻿using System;
using System.Collections.Generic;
using System.IO;

namespace wordcount
{
    public class WordCounter
    {
        private readonly char[] sep = " \\\n\t\"$'!,?;.:-_**+=)([]{}<>/@&%€#".ToCharArray();

        public WordCounter()
        {
        }

        //Return a dictionary containing all words (as the key)in the file
        // [f] and the value is the number of occurrences of the key in file.
        Dictionary<string, int> CountFile(FileInfo f)
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
            var content = File.ReadAllLines(f.FullName);
            foreach (var line in content)
            {
                foreach (var aWord in line.Split(sep, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (res.ContainsKey(aWord))
                        res[aWord]++;
                    else
                        res.Add(aWord, 1);
                }
            }

            return res;
        }

        // Return a dictionary of all the words (the key) in the files contained
        // in the directory [dir]. Only files with an extension in
        // [extensions] is read. The value part of the return value is
        // the number of occurrences of the key.
        public Dictionary<string, int> Crawl(DirectoryInfo dir, List<string> extensions) {
            Dictionary<string, int> res = new Dictionary<string, int>();

            Console.WriteLine("Crawling " + dir.FullName);

            foreach (var file in dir.EnumerateFiles())
                if (extensions.Contains(file.Extension))
                      AddDictionary(res, CountFile(file));
           
            foreach (var d in dir.EnumerateDirectories())
                AddDictionary(res, Crawl(d, extensions));

            return res;
        }

        // add [other] to res.
        private void AddDictionary(Dictionary<string, int> res, Dictionary<string, int> other)
        {
            foreach (KeyValuePair<String, int> p in other)
            {
                if (res.ContainsKey(p.Key))
                    res[p.Key] += p.Value;
                else
                    res.Add(p.Key, p.Value);
            }
        }
    }
}
