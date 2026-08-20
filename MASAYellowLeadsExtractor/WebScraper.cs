using System;
using System.Collections.Generic;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

// Token: 0x02000005 RID: 5
public static class WebScraper
{
	// Token: 0x06000017 RID: 23 RVA: 0x00002B00 File Offset: 0x00000D00
	private static void EnsureJs(WebView wv)
	{
		if (wv == null)
		{
			return;
		}
		object initLock = WebScraper._initLock;
		lock (initLock)
		{
			bool ok = false;
			try
			{
				object t = wv.EvalScript("typeof window.__ws_q");
				ok = t != null && t.ToString() == "function";
			}
			catch
			{
				ok = false;
			}
			if (!ok)
			{
				wv.EvalScript("\n(function(){\n  try{\n    window.__ws_q = function(sel, root){\n      root = root || document;\n      try { return root.querySelectorAll(sel); } catch(e){ return []; }\n    };\n    window.__ws_parent = function(el){\n      return el ? el.parentNode : null;\n    };\n    window.__ws_invoke = function(el, fn){\n      try { return (el && el[fn]) ? el[fn]() : null; } catch(e){ return null; }\n    };\n  }catch(e){}\n})();");
			}
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002B88 File Offset: 0x00000D88
	public static List<Element> QuerySelectorAll(WebView wv, string cssSelector)
	{
		WebScraper.EnsureJs(wv);
		object res = null;
		try
		{
			string text = "__ws_q";
			object[] array = new object[2];
			array[0] = cssSelector;
			res = wv.InvokeFunction(text, array);
		}
		catch
		{
			try
			{
				WebScraper.EnsureJs(wv);
				string text2 = "__ws_q";
				object[] array2 = new object[2];
				array2[0] = cssSelector;
				res = wv.InvokeFunction(text2, array2);
			}
			catch
			{
				return new List<Element>();
			}
		}
		return WebScraper.ToList((res == null) ? null : JSObject.CastTo<Collection<Element>>(res));
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002C10 File Offset: 0x00000E10
	public static List<Element> QuerySelectorAll(WebView wv, Element root, string cssSelector)
	{
		WebScraper.EnsureJs(wv);
		object res = null;
		try
		{
			res = wv.InvokeFunction("__ws_q", new object[] { cssSelector, root });
		}
		catch
		{
			try
			{
				WebScraper.EnsureJs(wv);
				res = wv.InvokeFunction("__ws_q", new object[] { cssSelector, root });
			}
			catch
			{
				return new List<Element>();
			}
		}
		return WebScraper.ToList((res == null) ? null : JSObject.CastTo<Collection<Element>>(res));
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002CA0 File Offset: 0x00000EA0
	private static string JsQuote(string s)
	{
		if (s == null)
		{
			return "''";
		}
		return "'" + s.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "")
			.Replace("\n", "") + "'";
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002D02 File Offset: 0x00000F02
	private static string CssClassSelector(string className)
	{
		if (string.IsNullOrEmpty(className))
		{
			return "";
		}
		return className.Replace("\\", "\\\\").Replace("'", "\\'");
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002D34 File Offset: 0x00000F34
	private static List<Element> ToList(Collection<Element> col)
	{
		if (col == null)
		{
			return new List<Element>();
		}
		int i = col.length;
		List<Element> list = new List<Element>(i);
		for (int j = 0; j < i; j++)
		{
			Element el = col[j];
			if (el != null)
			{
				list.Add(el);
			}
		}
		return list;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002D77 File Offset: 0x00000F77
	public static List<Element> GetElementsByTag(WebView webView, string tag)
	{
		if (webView == null || string.IsNullOrEmpty(tag))
		{
			return new List<Element>();
		}
		return WebScraper.QuerySelectorAll(webView, tag);
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002D91 File Offset: 0x00000F91
	public static List<Element> GetElementsByTag(WebView webView, Element element, string tag)
	{
		if (webView == null || element == null || string.IsNullOrEmpty(tag))
		{
			return new List<Element>();
		}
		return WebScraper.QuerySelectorAll(webView, element, tag);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002DB0 File Offset: 0x00000FB0
	public static Element GetParent(WebView webView, Element elem)
	{
		if (webView == null || elem == null)
		{
			return null;
		}
		WebScraper.EnsureJs(webView);
		object res = webView.InvokeFunction("__ws_parent", new object[] { elem });
		if (res == null)
		{
			return null;
		}
		JSObject jsObj = res as JSObject;
		if (jsObj == null)
		{
			return null;
		}
		return JSObject.CastTo<Element>(jsObj);
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002DF8 File Offset: 0x00000FF8
	public static void InvokeMember(WebView webView, Element elem, string funcName)
	{
		if (webView == null || elem == null || string.IsNullOrEmpty(funcName))
		{
			return;
		}
		try
		{
			WebScraper.EnsureJs(webView);
			webView.InvokeFunction("__ws_invoke", new object[] { elem, funcName });
		}
		catch
		{
		}
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002D77 File Offset: 0x00000F77
	public static List<Element> GetElements(WebView webView, string tag)
	{
		if (webView == null || string.IsNullOrEmpty(tag))
		{
			return new List<Element>();
		}
		return WebScraper.QuerySelectorAll(webView, tag);
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002E4C File Offset: 0x0000104C
	public static List<Element> GetElements(WebView webView, string tag, string className, bool exactlyMatch = true)
	{
		if (webView == null || string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(className))
		{
			return new List<Element>();
		}
		string selector;
		if (exactlyMatch)
		{
			if (className.IndexOf(' ') >= 0)
			{
				selector = tag + "[class=" + WebScraper.JsQuote(className) + "]";
			}
			else
			{
				selector = tag + "." + WebScraper.CssClassSelector(className);
			}
		}
		else
		{
			selector = tag + "[class*=" + WebScraper.JsQuote(className) + "]";
		}
		return WebScraper.QuerySelectorAll(webView, selector);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002ECC File Offset: 0x000010CC
	public static List<Element> GetElements(WebView webView, string tag, string[] classNames, bool exactlyMatch = true)
	{
		if (webView == null || string.IsNullOrEmpty(tag) || classNames == null || classNames.Length == 0)
		{
			return new List<Element>();
		}
		List<Element> result = new List<Element>();
		HashSet<int> seen = new HashSet<int>();
		foreach (string cn in classNames)
		{
			if (!string.IsNullOrEmpty(cn))
			{
				foreach (Element el in WebScraper.GetElements(webView, tag, cn, exactlyMatch))
				{
					int j = el.GetHashCode();
					if (seen.Add(j))
					{
						result.Add(el);
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002D91 File Offset: 0x00000F91
	public static List<Element> GetElements(WebView webView, Element element, string tag)
	{
		if (webView == null || element == null || string.IsNullOrEmpty(tag))
		{
			return new List<Element>();
		}
		return WebScraper.QuerySelectorAll(webView, element, tag);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002F78 File Offset: 0x00001178
	public static List<Element> GetElements(WebView webView, Element element, string tag, string className, bool exactlyMatch = true)
	{
		if (webView == null || element == null || string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(className))
		{
			return new List<Element>();
		}
		string selector;
		if (exactlyMatch)
		{
			if (className.IndexOf(' ') >= 0)
			{
				selector = tag + "[class=" + WebScraper.JsQuote(className) + "]";
			}
			else
			{
				selector = tag + "." + WebScraper.CssClassSelector(className);
			}
		}
		else
		{
			selector = tag + "[class*=" + WebScraper.JsQuote(className) + "]";
		}
		return WebScraper.QuerySelectorAll(webView, element, selector);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002FFC File Offset: 0x000011FC
	public static List<Element> GetElements(WebView webView, Element element, string tag, string[] classNames)
	{
		if (webView == null || element == null || string.IsNullOrEmpty(tag) || classNames == null || classNames.Length == 0)
		{
			return new List<Element>();
		}
		List<Element> result = new List<Element>();
		HashSet<int> seen = new HashSet<int>();
		foreach (string cn in classNames)
		{
			if (!string.IsNullOrEmpty(cn))
			{
				foreach (Element el in WebScraper.GetElements(webView, element, tag, cn, true))
				{
					int j = el.GetHashCode();
					if (seen.Add(j))
					{
						result.Add(el);
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x000030AC File Offset: 0x000012AC
	public static List<Element> GetElements(WebView webView, Element element, string[] tags, string className)
	{
		if (webView == null || element == null || tags == null || tags.Length == 0 || string.IsNullOrEmpty(className))
		{
			return new List<Element>();
		}
		List<Element> result = new List<Element>();
		HashSet<int> seen = new HashSet<int>();
		foreach (string tag in tags)
		{
			if (!string.IsNullOrEmpty(tag))
			{
				foreach (Element el in WebScraper.GetElements(webView, element, tag, className, true))
				{
					int j = el.GetHashCode();
					if (seen.Add(j))
					{
						result.Add(el);
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x0000315C File Offset: 0x0000135C
	public static List<Element> GetElementsByAttribute(WebView webView, Element root, string tag, string attribute, string attrVal, bool exactlyMatch = true)
	{
		if (webView == null || root == null || string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(attribute) || attrVal == null)
		{
			return new List<Element>();
		}
		string selector = (exactlyMatch ? string.Concat(new string[]
		{
			tag,
			"[",
			attribute,
			"=",
			WebScraper.JsQuote(attrVal),
			"]"
		}) : string.Concat(new string[]
		{
			tag,
			"[",
			attribute,
			"*=",
			WebScraper.JsQuote(attrVal),
			"]"
		}));
		return WebScraper.QuerySelectorAll(webView, root, selector);
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00003204 File Offset: 0x00001404
	public static List<Element> GetElementsByAttribute(WebView webView, string tag, string attribute, string attrVal, bool exactlyMatch = true)
	{
		if (webView == null || string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(attribute) || attrVal == null)
		{
			return new List<Element>();
		}
		string selector = (exactlyMatch ? string.Concat(new string[]
		{
			tag,
			"[",
			attribute,
			"=",
			WebScraper.JsQuote(attrVal),
			"]"
		}) : string.Concat(new string[]
		{
			tag,
			"[",
			attribute,
			"*=",
			WebScraper.JsQuote(attrVal),
			"]"
		}));
		return WebScraper.QuerySelectorAll(webView, selector);
	}

	// Token: 0x0600002A RID: 42 RVA: 0x000032A3 File Offset: 0x000014A3
	public static List<Element> GetChildren(WebView webView, Element elem)
	{
		if (webView == null || elem == null)
		{
			return new List<Element>();
		}
		return WebScraper.QuerySelectorAll(webView, elem, "*");
	}

	// Token: 0x04000009 RID: 9
	private static readonly object _initLock = new object();

	// Token: 0x0400000A RID: 10
	private static readonly HashSet<int> _initedWebViews = new HashSet<int>();
}
