var PROTO_PATH = __dirname + '/../protos/stream.proto';

var fs = require('fs'),
    http = require('http'),
    WebSocket = require('ws');

var socketServer = new WebSocket.Server({port: 8082, perMessageDeflate: false});
socketServer.connectionCount = 0;
socketServer.on('connection', function(socket, upgradeReq) {
	socketServer.connectionCount++;
	console.log(
		'New WebSocket Connection: ',
		(upgradeReq || socket.upgradeReq).socket.remoteAddress,
		(upgradeReq || socket.upgradeReq).headers['user-agent'],
		'('+socketServer.connectionCount+' total)'
	);
	socket.on('close', function(code, message){
		socketServer.connectionCount--;
		console.log(
			'Disconnected WebSocket ('+socketServer.connectionCount+' total)'
		);
	});
});
socketServer.broadcast = function(data) {
	socketServer.clients.forEach(function each(client) {
		if (client.readyState === WebSocket.OPEN) {
			client.send(data);
		}
	});
};

var grpc = require('@grpc/grpc-js');
var protoLoader = require('@grpc/proto-loader');
var packageDefinition = protoLoader.loadSync(
    PROTO_PATH,
    {keepCase: true,
     longs: String,
     enums: String,
     defaults: true,
     oneofs: true
    });
    
var protoDescriptor = grpc.loadPackageDefinition(packageDefinition);

function getStreamBytes(call, callback) {
    call.on('data', function(data) {
        console.log(data.data)
        socketServer.broadcast(data.data);
    });
    call.on('end', function() {
        console.log('close');
    });
  }

function getServer() {
    var server = new grpc.Server();
    server.addService(protoDescriptor.Streamer.service, {
        GetStreamBytes: getStreamBytes
    });
    return server;
}

var routeServer = getServer();
routeServer.bindAsync('localhost:50051', grpc.ServerCredentials.createInsecure(), () => {
  routeServer.start();
});