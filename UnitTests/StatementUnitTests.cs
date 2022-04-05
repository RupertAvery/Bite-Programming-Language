#define USE_NEW_PARSER

using Bite.Parser;
using Bite.Runtime;
using Bite.Runtime.CodeGen;
using Xunit;

namespace UnitTests
{

    public class StatementUnitTests
    {

        private BiteResult ExecStatements(string statements)
        {
#if USE_NEW_PARSER
            var compiler = new BITECompiler();
#else
            var compiler = new Compiler(true);
#endif
            var program = compiler.CompileStatements(statements);
            return program.Run();
        }


        [Fact]
        public void VariableDeclaration()
        {
            var result = ExecStatements("var a;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
        }

        [Fact]
        public void VariableDefintition()
        {
            var result = ExecStatements("var a = 12345; a;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(12345, result.ReturnValue.NumberData);
        }

        #region Functions
        [Fact]
        public void FunctionDefinition()
        {
            var result = ExecStatements("function foo(a) { a = a + 1; }");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
        }

        [Fact]
        public void FunctionDefinitionWithReturn()
        {
            var result = ExecStatements("function foo(a) { a = a + 1; return a; }");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
        }

        [Fact]
        public void FunctionCall()
        {
            var result = ExecStatements("function foo(a) { a = a + 1; } foo(1);");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
        }

        [Fact]
        public void FunctionCallNoReturnShouldThrowErrorWhenAssigned()
        {
            var result = ExecStatements("function foo(a) { a = a + 1; } var a = 1; a = foo(a);");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
        }

        [Fact]
        public void FunctionCallWithReturn()
        {
            var result = ExecStatements("function foo(a) { a = a + 1; return a; } var a = foo(1); a;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(2, result.ReturnValue.NumberData);
        }

        [Fact]
        public void FunctionCallWithMultipleArguments()
        {
            var result = ExecStatements("function foo(a, b) { return a + b; } var a = foo(1, 2); a;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(3, result.ReturnValue.NumberData);
        }

        [Fact]
        public void FunctionCallWith3Arguments()
        {
            var result = ExecStatements("function foo(a, b, c) { return a + b + c; } var a = foo(1,2,3); a;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(6, result.ReturnValue.NumberData);
        }

        [Fact]
        public void NestedFunctionCall()
        {
            var result = ExecStatements("function bar(a) { return a + a; } function foo(a) { return a + bar(a); } var a = foo(1); a;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(3, result.ReturnValue.NumberData);
        }

        [Fact]
        public void NestedFunctionCallReverseDeclaration()
        {
            var result = ExecStatements("function foo(a) { return a + bar(a); } function bar(a) { return a + a; } var a = foo(1); a;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(3, result.ReturnValue.NumberData);
        }

        [Fact]
        public void FunctionCallUseBeforeDeclaration()
        {
            var result = ExecStatements("var a = foo(1); function foo(a) { return a + bar(a); } function bar(a) { return a + a; } a;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(3, result.ReturnValue.NumberData);
        }

        #endregion



        #region Unary Operator Tests

        [Fact]
        public void PrefixIncrement()
        {
            var result = ExecStatements("var a = 7; ++a;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(8, result.ReturnValue.NumberData);
        }

        [Fact]
        public void PrefixDecrement()
        {
            var result = ExecStatements("var a = 7; --a;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(6, result.ReturnValue.NumberData);
        }

        [Fact]
        public void PostfixIncrement()
        {
            var result = ExecStatements("var a = 7; a++;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(7, result.ReturnValue.NumberData);
        }

        [Fact]
        public void PostfixDecrement()
        {
            var result = ExecStatements("var a = 7; a--;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(7, result.ReturnValue.NumberData);
        }


        [Fact]
        public void PrefixIncrementAsArgument()
        {
            var result = ExecStatements("function fn(a) { return a; } var a = 456; var b = fn(++a); b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(457, result.ReturnValue.NumberData);
        }


        [Fact]
        public void PrefixDecrementAsArgument()
        {
            var result = ExecStatements("function fn(a) { return a; } var a = 456; var b = fn(--a); b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(455, result.ReturnValue.NumberData);
        }

        [Fact]
        public void PostfixIncrementAsArgument()
        {
            var result = ExecStatements("function fn(a) { return a; } var a = 456; var b = fn(a++); b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(456, result.ReturnValue.NumberData);
        }

        [Fact]
        public void PostfixDecrementAsArgument()
        {
            var result = ExecStatements("function fn(a) { return a; } var a = 456; var b = fn(a++); b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(456, result.ReturnValue.NumberData);
        }


        #endregion

        #region Condition Tests


        [Fact]
        public void If()
        {
            var result = ExecStatements("var a = true; var b = 0; if (a) { b = 123; } b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(123, result.ReturnValue.NumberData);
        }

        [Fact]
        public void IfWithoutBraces()
        {
            var result = ExecStatements("var a = true; var b = 0; if (a) b = 123; b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(123, result.ReturnValue.NumberData);
        }

        [Fact]
        public void IfElseWithoutBracesTrue()
        {
            var result = ExecStatements("var a = true; var b = 0; if (a) b = 123; else b = 456; b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(123, result.ReturnValue.NumberData);
        }

        [Fact]
        public void IfElseWithoutBracesFalse()
        {
            var result = ExecStatements("var a = true; var b = 0; if (a) b = 123; else b = 456; b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(123, result.ReturnValue.NumberData);
        }


        [Fact]
        public void IfElseTrue()
        {
            var result = ExecStatements("var a = true; var b = 0; if (a) { b = 123; } else { b = 456; } b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(123, result.ReturnValue.NumberData);
        }

        [Fact]
        public void IfElseFalse()
        {
            var result = ExecStatements("var a = false; var b = 0; if (a) { b = 123; } else { b = 456; } b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(456, result.ReturnValue.NumberData);
        }

        [Fact]
        public void IfElseIfTrue()
        {
            var result = ExecStatements("var a = \"foo\"; var b = \"bar\"; var c = 789; if (a == \"foo\") { c = 123; } else if (b == \"bar\") { c = 456; } c;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(123, result.ReturnValue.NumberData);
        }

        [Fact]
        public void IfElseIfFalseThenTrue()
        {
            var result = ExecStatements("var a = \"fee\"; var b = \"bar\"; var c = 789; if (a == \"foo\") { c = 123; } else if (b == \"bar\") { c = 456; } c;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(456, result.ReturnValue.NumberData);
        }

        [Fact]
        public void IfElseIfFalseThenFalse()
        {
            var result = ExecStatements("var a = \"fee\"; var b = \"beer\"; var c = 789; if (a == \"foo\") { c = 123; } else if (b == \"bar\") { c = 456; } c;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(789, result.ReturnValue.NumberData);
        }

        [Fact]
        public void IfElseIfElse()
        {
            var result = ExecStatements("var a = \"fee\"; var b = \"beer\"; var c = 789; if (a == \"foo\") { c = 123; } else if (b == \"bar\") { c = 456; } else { c = 111; } c;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(111, result.ReturnValue.NumberData);
        }


        [Fact]
        public void TernaryTrue()
        {
            var result = ExecStatements("var a = true; var b = a ? 123 : 456; b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(123, result.ReturnValue.NumberData);
        }

        [Fact]
        public void TernaryFalse()
        {
            var result = ExecStatements("var a = false; var b = a ? 123 : 456; b;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(456, result.ReturnValue.NumberData);
        }

        #endregion


        #region Loop Tests
        [Fact]
        public void ForLoopScope()
        {
            var result = ExecStatements("var j = 0; for (var i = 0; i < 10; i++) { j++; i++; } j;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(10, result.ReturnValue.NumberData);
        }

        [Fact]
        public void ForLoop()
        {
            var result = ExecStatements("var j = 0; for (var i = 0; i < 10; i++) { j++;  } j;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(10, result.ReturnValue.NumberData);
        }

        [Fact]
        public void ForLoopDecrement()
        {
            var result = ExecStatements("var j = 0; for (var i = 10; i > 0; i--) { j++; } j;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(10, result.ReturnValue.NumberData);
        }


        [Fact]
        public void ForLoopBreak()
        {
            var result = ExecStatements("var j = 0; for (var i = 0; i < 10; i++) { j++; if (j == 6) { break; } } j;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(6, result.ReturnValue.NumberData);
        }

        [Fact]
        public void ForEverInitLoopBreak()
        {
            var result = ExecStatements("for (var i = 0;;) { i++; if (i == 10) { break; } } i;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(10, result.ReturnValue.NumberData);
        }

        [Fact]
        public void ForEverLoopBreak()
        {
            var result = ExecStatements("var i = 0; for (;;) { i++; if (i == 10) { break; } } i;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(10, result.ReturnValue.NumberData);
        }

        [Fact]
        public void ForMultipleVarAndIterator()
        {
            var result = ExecStatements("var a = 0; for (var i = 0, j = 0; i < 10; i++, j++ ) { a += j + i; } a;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(90, result.ReturnValue.NumberData);
        }


        [Fact]
        public void WhileLoop()
        {
            var result = ExecStatements("var i = 0; while(i < 10) { i++; } i;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(10, result.ReturnValue.NumberData);
        }

        [Fact]
        public void WhileTrueLoopBreak()
        {
            var result = ExecStatements("var i = 0; while(true) { i++; if (i == 5) { break; } } i;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(5, result.ReturnValue.NumberData);
        }

        [Fact]
        public void WhileFalseNeverExecutes()
        {
            var result = ExecStatements("var i = 12345; while(false) { i++; } i;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(12345, result.ReturnValue.NumberData);
        }

        #endregion


        #region Classes

        [Fact]
        public void ClassFields()
        {
            var statements = @"class TestClass
            {
                var x = 5;
            }

            var a = new TestClass();

            a.x;";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(5, result.ReturnValue.NumberData);
        }

        [Fact]
        public void ClassConstructorArguments()
        {
            var statements = @"class TestClass
            {
                var x = 5;

                function TestClass(n)
                {
                    x = n;
                }
            }

            var a = new TestClass(150);

            a.x;";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(150, result.ReturnValue.NumberData);
        }

        [Fact]
        public void ClassInstanceAsArgument()
        {
            var statements = @"class TestClass
            {
                var x = 5;
                function TestClass(n)
                {
                    x = n;
                }
            }

            function TestFunction(n)
            {
                n.x = 10;
            }

            var a = new TestClass(150);

            TestFunction(a);

            a.x;";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(10, result.ReturnValue.NumberData);
        }

        #endregion

        #region Assignment
        [Fact]
        public void ArithmeticAssignment()
        {
            var statements = @"var a = 1;
            a += 2;
            a;";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(3, result.ReturnValue.NumberData);
        }

        [Fact]
        public void ArithmeticAssignmentAssignment()
        {
            var statements = @"var a = 1;
            var b = a += 2;
            b;";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(3, result.ReturnValue.NumberData);
        }

        [Fact]
        public void PostFixReturnFromFunction()
        {
            // While this code probably doesn't make sense, 
            // i.e. you never return a postix, it is still valid code
            var result = ExecStatements("function fn(a) { return a++; } fn(1);");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(1, result.ReturnValue.NumberData);
        }

        [Fact]
        public void PostFixReturn()
        {
            // While this code probably doesn't make sense, 
            // i.e. you never return a postix, it is still valid code
            var result = ExecStatements("var a = 1; a++;");
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(1, result.ReturnValue.NumberData);
        }

        [Fact]
        public void PostFixAssignment()
        {
            var statements = @"var a = 1;
            var b = a++;
            b;";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(1, result.ReturnValue.NumberData);
        }

        [Fact]
        public void PostFixInBinaryOperationAssignment()
        {
            var statements = @"var a = 1;
            var b = a++ + 2;
            b;";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(3, result.ReturnValue.NumberData);
        }


        [Fact]
        public void PostFixMultiple()
        {
            var statements = @"var a = 1;
            a++;
            a++;
            a++;
            a++;
            a;";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(5, result.ReturnValue.NumberData);
        }

        [Fact]
        public void PostFixArgumentMultiple()
        {
            var statements = @"
            function foo(a){
              // nop
            }
            var a = 1;
            foo(a++);
            foo(a++);
            foo(a++);
            foo(a++);
            a;";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(5, result.ReturnValue.NumberData);
        }

        [Fact]
        public void MultiArgumentPostFix()
        {
            var statements = @"
            function foo(a, b){
              return a + b;
            }
            var a = 1;
            var b = 2;
            foo(a++, b++);";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(3, result.ReturnValue.NumberData);
        }
        
        [Fact]
        public void AfterMultiPostFix()
        {
            var statements = @"
            var a = 1;
            var b = 2;
            a++; 
            b++;
            a + b;";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(5, result.ReturnValue.NumberData);
        }

        [Fact]
        public void AfterMultiArgumentPostFix()
        {
            var statements = @"
            function foo(a, b){
              return a + b;
            }
            var a = 1;
            var b = 2;
            foo(a++, b++);
            a + b;";

            var result = ExecStatements(statements);
            Assert.Equal(BiteVmInterpretResult.InterpretOk, result.InterpretResult);
            Assert.Equal(5, result.ReturnValue.NumberData);
        }

        #endregion

    }
}
