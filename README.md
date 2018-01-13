# SecretSanta
C# web api implementing basic software with login, usergroups, user matching 

the API covers the following end-points

#1
POST ~/users
Header: None
Body:
{ "username" : "...",
  "displayName" : "...",
  "password" : "hash" }
Success:
201. Created
	Header : None
	Body: { "displayName" : "..." }
Error:
400 Bad request
409 Conflict - неуникално име 

#2
POST ~/logins
Header: None
Body: 
{ "username" : "...",
  "password" : "hash" }
Success:
201. Created
	Header : None
	Body: { "authToken" : "..." }
Error:
400 Bad request
404 Not found
401 Unauthorized - грешен username или парола

#3
DELETE ~/logins/{username}
Header : {"authToken" : "token" }
Body: None
Success
204 No content
	Header: None
	Body: None
Error
400 Bad request
404 Not found - вече отписан потребител

#4
GET ~/users?skip={s}&take={t}&order={Asc|Desc}&search={phrase}
Header : {"authToken" : "token" }
Success:
200 OK.
	Header: None
	Body: [ {"username" : "..." }, {...} ] 
Error:
400 bad request

#5
GET ~/users/{username}
Header : {"authToken" : "token" } 
Success:
200 Ok.
	Header: None
	Body: {"username" : "..." , "displayName" : "..." } 
Error:
400 Bad request
404 Not found

#6
POST ~/groups 
Header : {"authToken" : "..." } 
Body : {"groupName" : "..."}
Success:
201 Created.
Header: None
Body: { "groupName" : "..." , "adminName": "..."}
Error:
400 bad request
409 content - името не е уникално

#7
POST ~/usrs/{username}/invitations
Header : {"authToken" : "..." }
Body: 
{"groupName" : "...",
 "date" : "...",
 "adminName" : "..." }
Success:
201 Created
	Header : None
	Body : {"id": "..."}
Error:
400 bad request
403 Forbiden  - покана за група, на която не е администратор
404 Not found - не съществува потребител/група
409 Conflict - потребителя има покана

#8
GET ~/users/{username}/invitations?skip={s}&take={t}&order={A|D} 
Header : {"authToken" : "..." }
Success:
200 Ok.
Header: None
Body: [{"groupName" : "...", "date" : "...", "adminName" : "..." }, ...]
Error:
400 Bad request
403 Forbiden - достъпване на чужди покани

#9a
POST ~/groups/{groupName}/participants
Header : {"authToken" : "..." }
Body : {"username" : "..." }
Success: 
200 Created
Error :
400 Bad request
403 Forbiden - нямаме покана за тази група

#9c
DELETE ~/users/{username}/invitations/{id}
Header : {"authToken" : "..." }
Body : None
Success: 
204 No content
	Header: None
	Body: None
Error:
400 Bad request
409 Not found - изтрива липсваща покана

#10
POST(или пък PUT) ~/groups/{groupname}/links
Header : {"authToken" : "..." }
Body : No

Success: 
 201. Created no body no headers
Error: 
 400 bad request 
 404 Not found - няма потребител в групата с username подаден в body-то на request-a
 403 Forbiden - не сме създател на въпросната група, за която правим свързване
 412 Precondition Failed - се опитаме да стартираме процес с един член ( точка е)

#11
GET ~/users/{username}/groups?skip={s}&take={t}
Header : {"authToken" : "..." }
Success:
200 Ok.
	Header: none
	Body: [ ... ]
Error
 400 Bad request
 
#12
GET ~users/{username}/groups/{groupname}/links
Header: {"authToken" : "..." }
Success: 
 200 OK
	Header : None 
	Body : {"reciever" : "username" } 
 Error:
 400 bad request
 404 Not found - процесът още не е стартиран от създателя на групата ( точка б)

#13
GET ~/groups/{groupname}/participants
Header: {"authToken" : "..." }
Success:
200 Ok
	Header : None 
	Body : [...]
Error:
400 Bad request
403 Forbiden - побителят не е администратор

#14
DELETE ~/groups/{groupName}/participants/{participantUsername}
Header: {"authToken" : "..." }
Body: None
Success:
204 No content
	Header: None
	Body: None
Error:
400 Bad request
404 Not found - няма такъв участник
403 Forbiden - побителят не е администратор
