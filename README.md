Practical Test .NET Software Developer ke PT. Esphere Techno Corindo

Duration: 1,5 Hours max
Objective: Make meeting room booking application in .NET as a backend

Requirement:
Create repository on Github named “test_technical_code”
Make it public
Commit both frontend and backend on the same repository

Backend (.NET core):
Store data on SQL Lite or SQL Server.
Apply clean OOP principles.
Provide Rest API to booking meeting rooms with suggestions of available slots.
Make 2 meeting rooms. Meeting room A fits up to 5 people. Meeting room B fits up to 10 people.
By default
Meeting Room A booked at 7 to 9AM, 9:45 to 10:35AM, and 2 to 3PM
Meeting Room B booked at 8 to 10AM, 10:45 to 12:00PM, and 2 to 3 PM

Frontend:
Use javascript fetch to communicate with the backend.
Provide input for name, time (in combobox every 15 minutes from 7AM to 4:30PM), duration (in combo box every 15 minutes until 2 hours), and number of people (max 10 people).
Show the meeting room schedule of both of them in a table under the form.
If time, duration, and number of people is not available, reject and suggest different times.
Let user click on the suggestion to fill in the form automatically
If available, book the meeting room and update the table.

Input test:
Time 8AM, duration 45minutes, 5 people.
Expectedly rejected and suggested 9AM in Room A and 10AM Room B.
Time 10AM, duration 1 hour, 7 peoples
Expected rejected and suggested 12PM in Room B
Time 4PM, duration 1 hour, 1 people
Expectedly rejected because of why would you meeting alone
Time 4PM, duration 1 hour, 11 people
Expectedly rejected because the meeting fit up to 10 people only
Time 4PM, duration 1 hour, 5 people
Expectedly successfully booked to Room A

