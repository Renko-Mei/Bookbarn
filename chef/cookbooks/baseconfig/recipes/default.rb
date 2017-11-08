# Make sure the Apt package lists are up to date, so we're downloading versions that exist.

execute 'get gpg key' do 
  command 'curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg'
end 

execute 'add key to trusted' do 
  command 'sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg'
end 

execute 'move dotnet package to apt-sources' do 
  command "sudo sh -c 'echo \"deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-xenial-prod xenial main\" > /etc/apt/sources.list.d/dotnetdev.list'"
end 

cookbook_file "apt-sources.list" do
  path "/etc/apt/sources.list"
end
execute 'apt_update' do
  command 'apt-get update'
end

# Base configuration recipe in Chef.
package "wget"
package "ntp"
cookbook_file "ntp.conf" do
  path "/etc/ntp.conf"
end
execute 'ntp_restart' do
  command 'service ntp restart'
end

execute 'get_npm' do 
  command 'sudo apt-get install npm -y'
end 

execute 'update_npm' do 
  command 'sudo npm install -g npm'
end 

execute 'get_nodejs' do 
  command 'sudo apt-get install nodejs-legacy'
end 

execute 'get dotnet' do 
  command 'sudo apt-get install dotnet-sdk-2.0.0 -y'
end 

execute 'get deps' do
  command 'dotnet restore'
  cwd '/home/ubuntu/project'
end 
# TODO skip db stuff for now, just get hello world running
# execute 'run migrations' do 
#   command 'dotnet ef database update'
#   cwd '/home/ubuntu/project'
# end 

execute 'start dotnet' do 
  command 'nohup dotnet run > /dev/null 2>&1 &'
  cwd '/home/ubuntu/project'
end
