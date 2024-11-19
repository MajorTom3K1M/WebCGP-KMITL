# Use the Nginx image
FROM nginx:alpine

# Copy the contents of the public folder to the Nginx HTML directory
COPY publish/wwwroot /usr/share/nginx/html

# Expose port 80 to access the server
EXPOSE 5001

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]