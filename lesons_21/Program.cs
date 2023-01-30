using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Assembly assembly = Assembly.LoadFrom("test.dll");

        Type[] types = assembly.GetTypes();
        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute(typeof(CompilerGeneratedAttribute));
            if (attribute == null)
            {
                var methods = type.GetMethods(BindingFlags.DeclaredOnly|BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public|BindingFlags.Static);
                if (methods.Length == 0)
                    Console.WriteLine("Class" + type.Name + " without methods");
                else
                    Console.WriteLine("Class " + type.Name + " has next methods: ");
                foreach (var method in methods)
                {
                    string modificator = "";
                    if (method.IsPublic)
                        modificator += "public ";
                    else if (method.IsPrivate)
                        modificator += "private ";
                    else if (method.IsAssembly)
                        modificator += "internal ";
                    else if (method.IsFamily)
                        modificator += "protected ";
                    else if (method.IsFamilyAndAssembly)
                        modificator += "private protected ";
                    else if (method.IsFamilyOrAssembly)
                        modificator += "protected internal ";
                    if (method.IsStatic) modificator += "static ";
                    var parameters = method.GetParameters();
                    StringBuilder pms = new StringBuilder();
                    foreach (var parameter in parameters)
                    {
                        if (pms.ToString() == string.Empty)
                            pms.Append(parameter);
                        else
                            pms.Append("  "+parameter);
                    }
                    Console.WriteLine($"    "+modificator + " " + method.ReturnType.Name + " " + method.Name + (pms.ToString()));
                }
            }

        }
    }
}