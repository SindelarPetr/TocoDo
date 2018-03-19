using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TocoDo.UnitTests
{
	public static class CustomAsserts
	{
		public static void DictionariesAreEqual<K, V>(IDictionary<K, V> expectedDictionary, IDictionary<K, V> actualDictionary)
		{
			if (expectedDictionary == actualDictionary)
				return;

			if (expectedDictionary == null || actualDictionary == null)
				Assert.Fail("One of the given is null and the second is not."); // from the first if we know that those dictioanries are not the same, so if one of them is null, then the other is not.
			
			if(expectedDictionary.Count != actualDictionary.Count)
				Assert.Fail($"The count of items in both dictionaries is different. In the expected it is {expectedDictionary.Count} actual {actualDictionary.Count}");

			foreach (var pair in expectedDictionary)
			{
				if (!actualDictionary.ContainsKey(pair.Key))
				{
					Assert.Fail($"Failed assertation of an item, the key { pair.Key.ToString() } from expected dictionary is not present in the actual dictionary");
				}

				if(Comparer<V>.Default.Compare(actualDictionary[pair.Key], pair.Value) != 0)
				{
					Assert.Fail($"Failed assertation of an item, values missmatch, for the { pair.Key.ToString() } and the value {pair.Value.ToString() } from expected dictionary is not the same as value from the actual dictionary value {actualDictionary[pair.Key]}");
				}
			}
		}
	}
}
