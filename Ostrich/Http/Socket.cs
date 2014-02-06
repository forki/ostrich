﻿/*
 *  Licensed to the Apache Software Foundation (ASF) under one or more
 *  contributor license agreements.  See the NOTICE file distributed with
 *  this work for additional information regarding copyright ownership.
 *  The ASF licenses this file to You under the Apache License, Version 2.0
 *  (the "License"); you may not use this file except in compliance with
 *  the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 */
 using System;
using System.Collections.Generic;

namespace Ostrich.Http
{
    public static partial class Extensions
    {
        internal static ContinuationState<Unit> WriteChunk(this ISocket socket, ArraySegment<byte> chunk)
        {
            return socket.WriteChunkInternal(chunk).AsCoroutine<Unit>();
        }

        static IEnumerable<object> WriteChunkInternal(this ISocket socket, ArraySegment<byte> chunk)
        {
            int bytesWritten = 0;

            while (bytesWritten < chunk.Count)
            {
                var write = new ContinuationState<int>(socket.Write(chunk.Array, chunk.Offset + bytesWritten, chunk.Count - bytesWritten));
                yield return write;

                bytesWritten += write.Result;
            }

            yield return default(Unit);
        }
    }
}
