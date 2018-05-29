﻿using MiP.ObjectDump.Formatter;
using MiP.ObjectDump.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MiP.ObjectDump
{
    public static class Dump
    {
        public static string ViaJObject<T>(T item)
        {
            var serializer = new JsonSerializer()
            {
                Converters = { new StringEnumConverter() },
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling =  TypeNameAssemblyFormatHandling.Simple
            };

            JToken token = JToken.FromObject(item, serializer);
            using (StringWriter writer = new StringWriter())
            {
                JObjectFormatter formatter = new JObjectFormatter(writer);

                Html.WriteBeginHtml(writer, true, "Title TBD", Html.GetDefaultStyles());

                formatter.Format(token);

                Html.WriteEndHtml(writer);

                return writer.ToString();
            }
        }

        public static string ViaReflection(object item)
        {
            using (StringWriter writer = new StringWriter())
            {
                var formatter = new ReflectionFormatter(writer);

                Html.WriteBeginHtml(writer, true, "Title TBD", Html.GetDefaultStyles());

                formatter.Format(item);

                Html.WriteEndHtml(writer);

                return writer.ToString();
            }
        }

        public static string ToHtml(object item, int depth = 5)
        {
            using (StringWriter writer = new StringWriter())
            {
                var formatter = new HtmlFormatter(writer);

                Html.WriteBeginHtml(writer, true, "Title TBD", Html.GetDefaultStyles());

                Reflector reflector = new Reflector();
                var dobj = reflector.GetDObject(item, depth);

                formatter.WriteObject(dobj);

                Html.WriteEndHtml(writer);

                return writer.ToString();
            }
        }
    }
}
