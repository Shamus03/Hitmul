using NUnit.Framework;
using System;

namespace HitmulCore.Tests
{
    public class HitmulTests
    {
        [Test]
        public void FormatsStrings()
        {
            var text = "<span>html injection</span>";
            var html = new Hitmul($"<div>{text}</div>").ToHtml();
            Assert.That(html, Is.EqualTo("<div>&lt;span&gt;html injection&lt;/span&gt;</div>"));
        }

        [Test]
        public void FormatsNumbers()
        {
            var num = 123;
            var html = new Hitmul($"<div>{num}</div>").ToHtml();
            Assert.That(html, Is.EqualTo("<div>123</div>"));
        }

        [Test]
        public void ThrowsOnOtherObjects()
        {
            var some = new { something = "thing" };
            var html = new Hitmul($"<div>{some}</div>");
            Assert.That(html.ToHtml, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void HandlesNestedHitmul()
        {
            var inner = new Hitmul($"<span>inside</span>");
            var html = new Hitmul($"<div>{inner}</div>").ToHtml();
            Assert.That(html, Is.EqualTo("<div><span>inside</span></div>"));
        }

        [Test]
        public void EmptyHitmul()
        {
            Assert.That(default(Hitmul).ToHtml(), Is.EqualTo(""));
        }

        [Test]
        public void Raw()
        {
            var text = "<span>html injection</span>";
            var html = Hitmul.Raw($"<div>{text}</div>").ToHtml();
            Assert.That(html, Is.EqualTo("<div><span>html injection</span></div>"));
        }

        [Test]
        public void Append()
        {
            var first = "<first>";
            var second = "<second>";
            var html = new Hitmul($"<div>{first}</div>")
                .Append($"<div>{second}</div>")
                .ToHtml();
            Assert.That(html, Is.EqualTo("<div>&lt;first&gt;</div>\n<div>&lt;second&gt;</div>"));
        }

        [Test]
        public void AppendRaw()
        {
            var first = "<first>";
            var second = "<second>";
            var html = new Hitmul($"<div>{first}</div>")
                .AppendRaw("<div>{0}</div>", second)
                .ToHtml();
            Assert.That(html, Is.EqualTo("<div>&lt;first&gt;</div>\n<div>&lt;second&gt;</div>"));
        }
    }
}