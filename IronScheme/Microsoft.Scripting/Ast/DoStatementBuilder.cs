/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public License. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the  Microsoft Public License, please send an email to 
 * dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 *
 *
 * ***************************************************************************/
// TODO: Remove this
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Scripting.Utils;

namespace Microsoft.Scripting.Ast {
    public class DoStatementBuilder {
        private readonly Statement _body;
        private readonly SourceLocation _doLocation;
        private readonly SourceSpan _statementSpan;

        internal DoStatementBuilder(SourceSpan statementSpan, SourceLocation location, Statement body) {
            Contract.RequiresNotNull(body, "body");

            _body = body;
            _doLocation = location;
            _statementSpan = statementSpan;
        }

        public DoStatement While(Expression condition) {
            Contract.RequiresNotNull(condition, "condition");
            Contract.Requires(condition.Type == typeof(bool), "condition", "Condition must be boolean");

            return new DoStatement(_statementSpan, _doLocation, condition, _body);
        }
    }

    public static partial class Ast {
        public static DoStatementBuilder Do(params Statement[] body) {
            Contract.RequiresNotNullItems(body, "body");
            return new DoStatementBuilder(SourceSpan.None, SourceLocation.None, Block(body));
        }

        public static DoStatementBuilder Do(Statement body) {
            return new DoStatementBuilder(SourceSpan.None, SourceLocation.None, body);
        }

        public static DoStatementBuilder Do(SourceSpan statementSpan, SourceLocation location, Statement body) {
            return new DoStatementBuilder(statementSpan, location, body);
        }
    }
}
