Option Strict On
Option Explicit On


Public Class Cifrado

    Shared Property Key As Byte()   'lBz8YWVwX7grF/8HfFu4Q0Rar6QRfsF1hpG7W1qdioM=
    Shared Property IV As Byte() = Convert.FromBase64String("I7a1N/aD2Zkn0tX3tbqnIw==")

    Public Shared Function CifrarSHA512(ByVal Input As String) As String

        Dim sha As System.Security.Cryptography.SHA512 = New System.Security.Cryptography.SHA512CryptoServiceProvider
        Dim bytesToHash() As Byte
        Dim Output As String = Nothing

        bytesToHash = System.Text.Encoding.ASCII.GetBytes(Input)
        bytesToHash = sha.ComputeHash(bytesToHash)

        For Each b As Byte In bytesToHash
            Output += b.ToString("X2")
        Next

        Return Output

    End Function

    Public Shared Function CifrarAES(ByVal PlainText As String) As String

        Try

            Validar()

            'If Key.Length <> 32 Then
            '    Throw New Exception("Llave diferente a 32 caracters")
            'End If

            ' Create a new instance of the Aes
            ' class.  This generates a new key and initialization 
            ' vector (IV).
            Dim myAes As System.Security.Cryptography.Aes = System.Security.Cryptography.Aes.Create()

            ' Encrypt the string to an array of bytes.
            Dim encrypted As Byte() = EncryptStringToBytes_Aes(PlainText, Key, IV)

            Return Convert.ToBase64String(encrypted)




        Catch e As Exception
            Console.WriteLine("Error: {0}", e.Message)
            Return Nothing
        End Try

    End Function

    Public Shared Function DescifrarAES(ByVal Input As String) As String

        Try

            
            Validar()

            Dim original As String = Input

            ' Create a new instance of the Aes
            ' class.  This generates a new key and initialization 
            ' vector (IV).
            Dim myAes As System.Security.Cryptography.Aes = System.Security.Cryptography.Aes.Create()
            Dim encrypted As Byte() = Convert.FromBase64String(Input)

            ' Decrypt the bytes to a string.
            Dim roundtrip As String = DecryptStringFromBytes_Aes(encrypted, Key, IV)

            Return roundtrip

        Catch e As Exception
            Console.WriteLine("Error: {0}", e.Message)
            Return Nothing
        End Try

    End Function

    Shared Function EncryptStringToBytes_Aes(ByVal plainText As String, ByVal Key() As Byte, ByVal IV() As Byte) As Byte()
        ' Check arguments.
        If plainText Is Nothing OrElse plainText.Length <= 0 Then
            Throw New ArgumentNullException("plainText")
        End If
        If Key Is Nothing OrElse Key.Length <= 0 Then
            Throw New ArgumentNullException("Key")
        End If
        If IV Is Nothing OrElse IV.Length <= 0 Then
            Throw New ArgumentNullException("Key")
        End If
        Dim encrypted() As Byte
        ' Create an Aes object
        ' with the specified key and IV.
        Using aesAlg As System.Security.Cryptography.Aes = System.Security.Cryptography.Aes.Create()

            aesAlg.Key = Key
            aesAlg.IV = IV

            ' Create a decrytor to perform the stream transform.
            Dim encryptor As System.Security.Cryptography.ICryptoTransform = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV)
            ' Create the streams used for encryption.
            Using msEncrypt As New System.IO.MemoryStream()
                Using csEncrypt As New System.Security.Cryptography.CryptoStream(msEncrypt, encryptor, System.Security.Cryptography.CryptoStreamMode.Write)
                    Using swEncrypt As New System.IO.StreamWriter(csEncrypt)

                        'Write all data to the stream.
                        swEncrypt.Write(plainText)
                    End Using
                    encrypted = msEncrypt.ToArray()
                End Using
            End Using
        End Using

        ' Return the encrypted bytes from the memory stream.
        Return encrypted

    End Function 'EncryptStringToBytes_Aes

    Shared Function DecryptStringFromBytes_Aes(ByVal cipherText() As Byte, ByVal Key() As Byte, ByVal IV() As Byte) As String
        ' Check arguments.
        If cipherText Is Nothing OrElse cipherText.Length <= 0 Then
            Throw New ArgumentNullException("cipherText")
        End If
        If Key Is Nothing OrElse Key.Length <= 0 Then
            Throw New ArgumentNullException("Key")
        End If
        If IV Is Nothing OrElse IV.Length <= 0 Then
            Throw New ArgumentNullException("Key")
        End If
        ' Declare the string used to hold
        ' the decrypted text.
        Dim plaintext As String = Nothing

        ' Create an Aes object
        ' with the specified key and IV.
        Using aesAlg As System.Security.Cryptography.Aes = System.Security.Cryptography.Aes.Create()
            aesAlg.Key = Key
            aesAlg.IV = IV

            ' Create a decrytor to perform the stream transform.
            Dim decryptor As System.Security.Cryptography.ICryptoTransform = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV)

            ' Create the streams used for decryption.
            Using msDecrypt As New System.IO.MemoryStream(cipherText)

                Using csDecrypt As New System.Security.Cryptography.CryptoStream(msDecrypt, decryptor, System.Security.Cryptography.CryptoStreamMode.Read)

                    Using srDecrypt As New System.IO.StreamReader(csDecrypt)

                        ' Read the decrypted bytes from the decrypting stream
                        ' and place them in a string.
                        plaintext = srDecrypt.ReadToEnd()
                    End Using
                End Using
            End Using
        End Using

        Return plaintext

    End Function 'DecryptStringFromBytes_Aes 

    Public Shared Function Validar() As Boolean
        If Not Environment.GetEnvironmentVariable("consarkey") = Nothing Then
            Key = Convert.FromBase64String(Environment.GetEnvironmentVariable("consarkey"))
        Else
            Throw New Exception("Llave no encontrada")
        End If

        If Key.Length <> 32 Then
            Throw New Exception("Llave diferente a 32 caracteres")
        End If

        Return False

    End Function


    Public Shared Function ValidarKey() As Boolean

        If Environment.GetEnvironmentVariable("consarkey") = Nothing Then
            Return False
        Else
            Return True
        End If

    End Function

End Class

