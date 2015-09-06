using System.CodeDom.Compiler;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.CSharp;
using UnknownREEncrypter.Properties;

namespace UnknownREEncrypter
{
    public class Compiler
    {
        public static bool CompileFromSource(string source, string outputName, string resource, Icon icon)
        {
            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = true,
                OutputAssembly = outputName
            };

            // Add the necessary DLL's for the compiler
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Drawing.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            compilerParameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\PresentationFramework.dll");
            compilerParameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\PresentationCore.dll");
            compilerParameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\WindowsBase.dll");
            compilerParameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Xaml.dll");

            // Set the icon for the file ^^
            var tempFile = Path.GetTempFileName();
            using (Stream fileStream = File.OpenWrite(tempFile))
            {
                Resources.icon.Save(fileStream);
            }

            compilerParameters.CompilerOptions = @"/unsafe /win32icon:" + tempFile;

            compilerParameters.EmbeddedResources.Add(resource);

            CompilerResults compilerResults = new CSharpCodeProvider().CompileAssemblyFromSource(compilerParameters, source);
            
            if (compilerResults.Errors.Count <= 0)
            {
                File.Delete(tempFile);
                return true;
            }

            MessageBox.Show(
                $"The compiler has encountered {compilerResults.Errors.Count} errors",
                @"Errors while compiling", MessageBoxButtons.OK, MessageBoxIcon.Hand
                );
            
            foreach (CompilerError error in compilerResults.Errors)
            {
                MessageBox.Show($"{error.ErrorText}\nLine: {error.Line} - Column: {error.Column}\nFile: {error.FileName}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }            
            return false;
        }
    }
}