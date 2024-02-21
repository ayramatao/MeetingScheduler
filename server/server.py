from http.server import BaseHTTPRequestHandler, HTTPServer
import os
import yaml

host_name = "" #vårt hosename
server_port = 8080
base_directory = os.path.abspath(os.path.dirname(__file__))
yaml_file_path = "/home/server/server_files.yaml" #våre server-filer
log_file_path = "/home/server/logfile.log" # våre log-filer

class WebServer(BaseHTTPRequestHandler):
  def do_GET(self):
    self.send_response(200)
    self.send_header("Content-type", "text/html")
    self.end_headers()

  if self.path == "/":
    self.path = "index.html"
  try:
    with open(os.path.join(".", self.path[1:]), "rb") as file:
      content = file.read()
      self.wfile.write(content)

  except FileNotFoundError:
    self.wfile.write(bytes("File not found!", "utf-8")

  def do_POST(self):
    content_length = int(self.headers("Content-length"))
    post_data = self.rfile.read(content_length)
    try:
      data = yaml.safe_load(post_data.decode("utf-8"))

      with open(yaml_file_path, "w") as file:
        yaml.dump(data, file)

      self.send_response(200) # HTTP-Success
      self.send_header("Content-type", "text/plain")
      self.end_headers()
      self.wfile.write(bytes("Updated the database successfully with the client-side request data", "utf-8"))

    except Exception as e:
      self.send_response(400) # HTTP-Error
      self.send_header("Content-type", "text/plain")
      self.end_headers()
      self.wfile.write(bytes(f"Error: {str(e)}", "utf-8"))

# Driver code

web_server = HTTPServer((host_name, server_port), WebServer)
print(f"Server running on http://{host_name}/{server_port}")

try:
  web_server.server_forever()
except:
  web_server.server_close()
  print("Server stopped!")
  
