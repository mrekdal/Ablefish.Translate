exec web.GetWorkDone 'Martí';
exec web.GetWorkDone 'Huda';
GO

exec web.GetWorkDone 'zxs29rog9jp4';
GO

exec Web.GetWorkBatch 4, 'zxs29rog9jp4', 'ca', 'nb';
exec Web.GetWorkBatch 4, 'zxs29rog9jp4', 'da', 'nb';
GO

SELECT * FROM dbo.UserList;
SELECT * FROM dbo.WorkItem WHERE ProjectId = 4;
GO

EXEC dbo.AddWorkItem 4, 'Please', 'Please';
EXEC dbo.AddWorkItem 4, 'GoingHome', 'I am going home now.';
EXEC dbo.AddWorkItem 4, 'LetItBe', 'Let that be a lesson for you.';
EXEC dbo.AddWorkItem 4, 'VanceAndRubio', 'Vance and Rubio to meet with officials from Denmark and Greenland.';
EXEC dbo.AddWorkItem 4, 'TrumpThreats', 'There''s resounding frustration in Greenland as Trump''s threats ring loud.';
EXEC dbo.AddWorkItem 4, 'CompletelyBonkers', 'Completely Bonkers: Trump''s Greenland mining dreams collide with reality.';
EXEC dbo.AddWorkItem 4, 'FBISearch', 'FBI searches Washington Post reporter''s home.';
EXEC dbo.AddWorkItem 4, 'IranBodies', 'Iranian authorities are allegedly charging some people to retrieve bodies of loved ones killed in protests.';
EXEC dbo.AddWorkItem 4, 'LadyMoved', 'She moved from California to Sweden in search of a new adventure — she wasn’t prepared for the quiet.';
EXEC dbo.AddWorkItem 4, 'LadyMoved-1', 'There are few things Arabella Carey Adolfsson enjoys more than going fishing near her lakeside home in Sweden during the summertime, or getting her camera out and taking photographs of the natural beauty surrounding her.';
EXEC dbo.AddWorkItem 4, 'LadyMoved-2', 'She and her husband Stefan, a Swede, often take their boat out from Torpön, the island where they live, onto the waters of Lake Sommen, savoring the picturesque views of the surrounding fields, forests and cliffs.';
EXEC dbo.AddWorkItem 4, 'LadyMoved-3','“It’s gorgeous here,” Adolfsson, who was born and raised in San Diego, tells CNN Travel. “Sweden is beautiful. The lake is beautiful. The air is clean. There’s no traffic.”';
EXEC dbo.AddWorkItem 4, 'LadyMoved-4','Since moving to Scandinavia in 2022, after spending much of her life in California, she’s come to appreciate the rhythm of having four distinct seasons — though Swedish winters, she admits, “can be quite brutal.”';
EXEC dbo.AddWorkItem 4, 'SignIn-1','You need to be signed in to use this application.';
EXEC dbo.AddWorkItem 4, 'SignIn-2','You can sign in or create an account by clicking on the button in the top right corner.';
EXEC dbo.AddWorkItem 4, 'SignIn-3','After creating an account, please contact support to get your account authorized and to be assigned to projects.';
