const {parsed, error} = require('dotenv').config();

if (error) {
  console.log('dotenv error', error)
  throw error
} else {
  console.log('Loaded env variables')
  Object.keys(parsed).forEach(k => {
    console.log(`${k}: ${parsed[k]}`)
  })
  console.log('\nMachine-specific env variables')
  Object.keys(parsed).forEach(k => {
    console.log(`${k}: ${process.env[k]}`)
  })
  console.log('\nOverride ALREADY_SET_VAR with loaded variable')
  process.env.ALREADY_SET_VAR = parsed.ALREADY_SET_VAR
  console.log(`process.env.ALREADY_SET_VAR=${process.env.ALREADY_SET_VAR}`)
}

var http = require("http");
var express = require('express');
var app = express();

var mysql = require('mysql');

var bodyParser = require('body-parser');


var connection = mysql.createConnection(
{
	host     : process.env.RDS_HOSTNAME,
	user     : process.env.RDS_USERNAME,
	password : process.env.RDS_PASSWORD,
	port     : process.env.RDS_PORT,
	database : process.env.RDS_DB_NAME,
    dateStrings: 'date'

});

connection.connect(function(err){
	if(err) throw err
	console.log('connection...');
});

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
	extended : true
}));

var server = app.listen(3000, "0.0.0.0" , function(){
	var host = server.address().address;
	var port = server.address().port;

	console.log("example app listening at http://%s:%s" , host, port);
});


app.get('/userInfo/get' , function(req,res){
	
	var nickname = req.query.nickname;
	var sql = 'select * from UserInfo where nickname = ?';
	console.log("nick : " + nickname);
	connection.query(sql,[nickname],function(error, results, fields)
	{	
		if(error)
        {
            console.log(error);
            res.status(400).send(error);
        }
        else
        {
            console.log(results);
		    res.status(200).send(JSON.stringify(results));		
        }
		
	});
})
app.post('/userInfo/update/profile' , function(req,res)
{
	var nickname = req.body.nickname;
	var profile_skin = req.body.profile_skin;
	var profile_style = req.body.profile_style;

	var sql = 'update UserInfo set profile_skin = ?,profile_style = ? where nickname = ?';
	connection.query(sql,[profile_skin,profile_style,nickname],function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);

			res.status(200).send(JSON.stringify(results));
		}
				
	});
})

app.post('/userInfo/update' , function(req,res)
{
    var userInfo = req.body;
	var nickname = userInfo.nickname;
	
	var sql = 'update UserInfo SET ? where nickname = ?';
	connection.query(sql,[userInfo,nickname],function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);

			res.status(200).send(JSON.stringify(results));
		}
				
	});
})

app.post('/userInfo/insert' , function(req,res)
{
	var newUser = req.body;

	var sql = 'insert into UserInfo SET ?';

	connection.query(sql,newUser,function(error, results, fields)
	{	
		if(error){
			console.log(error);
			res.status(400).send(error);
		}
		else{
			console.log(results);
			res.status(200).send(JSON.stringify(results));
		}
				
	});

	
})