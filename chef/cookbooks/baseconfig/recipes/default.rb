# Make sure the Apt package lists are up to date, so we're downloading versions that exist.

execute 'get gpg key' do
  command 'curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg'
end

execute 'add key to trusted' do
  command 'mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg'
end

execute 'move dotnet package to apt-sources' do
  command "sh -c 'echo \"deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-xenial-prod xenial main\" > /etc/apt/sources.list.d/dotnetdev.list'"
end

execute 'add npm set up' do
  command 'curl -sL https://deb.nodesource.com/setup_8.x | sudo -E bash -'
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
package "nginx"
package "postgresql"

cookbook_file "ntp.conf" do
  path "/etc/ntp.conf"
end

execute 'ntp_restart' do
  command 'service ntp restart'
end

execute 'create db' do
  command 'echo "CREATE USER proj PASSWORD \'test\'; ALTER USER proj CREATEDB;" | sudo -u postgres psql'
end

execute 'get dotnet' do
  command 'sudo apt-get install dotnet-sdk-2.0.0 -y'
end

execute 'get dotnet deps' do
  command 'dotnet restore'
  cwd '/home/ubuntu/project'
end

execute 'get node' do
  command 'apt-get install -y nodejs'
end

execute 'get node deps' do
  command 'npm install'
  cwd '/home/ubuntu/project'
end

execute 'User model migration' do
  command 'dotnet ef migrations add UserModel -c AuthenticationContext'
  cwd '/home/ubuntu/project'
end

execute 'Other model migration' do
  command 'dotnet ef migrations add DefaultModels -c InitialModelsContext'
  cwd '/home/ubuntu/project'
end

execute 'Update db for user model' do
  command 'dotnet ef database update -c AuthenticationContext'
  cwd '/home/ubuntu/project'
end

execute 'Update db for other models' do
  command 'dotnet ef database update -c InitialModelsContext'
  cwd '/home/ubuntu/project'
end

# TODO uncomment this -- autoruns dotnet
# execute 'start dotnet' do
#   command 'nohup dotnet run > /dev/null 2>&1 &'
#   cwd '/home/ubuntu/project'
# end

execute 'start nginx' do
  command 'service nginx start'
end

cookbook_file 'nginx-default' do
  path '/etc/nginx/sites-available/default'
end

execute 'nginx_reload' do
  command 'nginx -s reload'
end
