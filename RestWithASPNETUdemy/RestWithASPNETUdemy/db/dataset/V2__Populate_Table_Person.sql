SET IDENTITY_INSERT [dbo].[person] ON;

INSERT INTO [dbo].[person] ([id], [first_name], [last_name], [gender], [address])
VALUES
    (1, 'Ayrton', 'Senna', 'Male', 'São Paulo - Brasil'),
    (2, 'Leonardo', 'da Vinci', 'Male', 'Anchiano - Italy'),
    (3, 'Mahatma', 'Gandhi', 'Male', 'Porbandar - India'),
    (4, 'Mohamed Ali', 'Gandhi', 'Male', 'Kentucky - USA'),
    (5, 'Nelson', 'Mandela', 'Male', 'Mvezo - South Africa');

SET IDENTITY_INSERT [dbo].[person] OFF;