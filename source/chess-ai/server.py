#!/usr/bin/env python3

from __future__ import absolute_import, division, print_function, unicode_literals

from http.server import BaseHTTPRequestHandler, HTTPServer
from stockfish import Stockfish
import os

import logging
from datetime import datetime
import socket    
hostname = socket.gethostname()    
IPAddr = socket.gethostbyname(hostname)    
print("Your Computer Name is:" + hostname)    
print("Your Computer IP Address is:" + IPAddr) 
print("-----------------------------------")
print("Initialising Stockfish")

dirname = os.path.dirname(__file__)
print(dirname)
filename = os.path.join(dirname, 'stockfish-10-linux/Linux/stockfish_10_x64')
stockfish = Stockfish(filename)
print("Stockfish Ready")

print("-----------------------------------")
class S(BaseHTTPRequestHandler):
    def _set_response(self):
        self.send_response(200)
        self.send_header('Content-type', 'text/html')
        self.end_headers()

    def do_POST(self):
        content_length = int(self.headers['Content-Length']) 
        post_data = self.rfile.read(content_length)
        logging.info("POST request,\nPath: %s\nHeaders:\n%s\n\nBody:\n%s\n",
        str(self.path), str(self.headers), post_data.decode('utf-8') )
        
        
        now = datetime.now()
        current_time = now.strftime("%H:%M:%S")
        self._set_response()
        
        print("Current Time =", current_time)
        passedMessage = post_data.decode('utf-8').replace("%2F", "/").replace("+"," ").replace("board=","")
        if (passedMessage == "test=test"):
            self.wfile.write("pong".encode('utf-8'))

        else:
            try:
                print(passedMessage)
                stockfish.set_fen_position(passedMessage)
                move = stockfish.get_best_move()
                print(move)        
                self.wfile.write(move.encode('utf-8'))
            except:
                self.wfile.write("Error".encode('utf-8'))



        #.encode('utf-8')
        

        
def run(server_class=HTTPServer, handler_class=S, port=3389):
    logging.basicConfig(level=logging.INFO)
    server_address = ('', port)
    print("Running on " + str(server_address))
    httpd = server_class(server_address, handler_class)
    logging.info('Starting http...\n')
    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        pass
    httpd.server_close()
    logging.info('Stopping http...\n')

if __name__ == '__main__':
    from sys import argv

    if len(argv) == 2:
        run(port=int(argv[1]))
    else:
        run()