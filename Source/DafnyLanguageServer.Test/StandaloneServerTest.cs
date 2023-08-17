using System;
using System.IO;
using System.Text;
using Microsoft.Dafny.LanguageServer.Workspace;
using Xunit.Abstractions;
using Xunit;

namespace Microsoft.Dafny.LanguageServer.IntegrationTest;

public class StandaloneServerTest {
  private readonly TextWriter output;

  public StandaloneServerTest(ITestOutputHelper output) {
    this.output = new WriterFromOutputHelper(output);
  }

  [Fact]
  public void OptionParsing() {
    var arguments = new[] { "--documents:verify=onsave", "--verifier:timelimit=3", "--ghost:markStatements=true" };
    var options = ServerProgram.GetOptionsFromArgs(output, TextReader.Null, arguments);
    Assert.Equal(VerifyOnMode.Save, options.Get(ServerCommand.Verification));
  }
}

public class WriterFromOutputHelper : TextWriter {
  private readonly ITestOutputHelper output;
  private bool failed = false;

  public WriterFromOutputHelper(ITestOutputHelper output) {
    this.output = output;
  }

  public override void Write(char value) {
    if (!failed) {
      failed = true;
      WriteLine("Error: tried to write a single character, which WriterFromOutputHelper does not support.");
    }
  }

  public override Encoding Encoding => Encoding.Default;

  public override void WriteLine(string value) {
    output.WriteLine(value);
  }

  public override void WriteLine(string format, params object[] arg) {
    output.WriteLine(format, arg);
  }
}
