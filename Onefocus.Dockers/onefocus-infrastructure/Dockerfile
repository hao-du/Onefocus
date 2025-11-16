FROM python:3.10-slim

# Install dependencies for all services
RUN apt-get update && apt-get install -y \
    curl \
    gnupg \
    supervisor \
    openjdk-11-jre-headless \
    && rm -rf /var/lib/apt/lists/*

# Install Redis
RUN curl -fsSL https://packages.redis.io/gpg | gpg --dearmor -o /usr/share/keyrings/redis-archive-keyring.gpg \
    && echo "deb [signed-by=/usr/share/keyrings/redis-archive-keyring.gpg] https://packages.redis.io/deb bullseye main" > /etc/apt/sources.list.d/redis.list \
    && apt-get update \
    && apt-get install -y redis-server \
    && rm -rf /var/lib/apt/lists/*

# Install RabbitMQ
RUN curl -fsSL https://packagecloud.io/rabbitmq/rabbitmq-server/gpgkey | gpg --dearmor -o /usr/share/keyrings/rabbitmq-archive-keyring.gpg \
    && echo "deb [signed-by=/usr/share/keyrings/rabbitmq-archive-keyring.gpg] https://packagecloud.io/rabbitmq/rabbitmq-server/debian bullseye main" > /etc/apt/sources.list.d/rabbitmq.list \
    && apt-get update \
    && apt-get install -y rabbitmq-server \
    && rm -rf /var/lib/apt/lists/*

# Install OpenSearch and OpenSearch Dashboards
RUN curl -fsSL https://artifacts.opensearch.org/publickeys/opensearch.pgp | gpg --dearmor -o /usr/share/keyrings/opensearch-archive-keyring.gpg \
    && echo "deb [signed-by=/usr/share/keyrings/opensearch-archive-keyring.gpg] https://artifacts.opensearch.org/releases/bundle/opensearch/2.x/apt stable main" > /etc/apt/sources.list.d/opensearch.list \
    && apt-get update \
    && apt-get install -y opensearch \
    && curl -o /tmp/opensearch-dashboards.deb https://artifacts.opensearch.org/releases/bundle/opensearch-dashboards/2.x/apt/pool/main/o/opensearch-dashboards/opensearch-dashboards_2.11.0_amd64.deb \
    && apt-get install -y /tmp/opensearch-dashboards.deb \
    && rm /tmp/opensearch-dashboards.deb \
    && rm -rf /var/lib/apt/lists/*

# Install Python dependencies for FastAPI
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copy FastAPI app
COPY app.py .

# Copy supervisord configuration
COPY supervisord.conf /etc/supervisor/conf.d/supervisord.conf

# Copy Redis configuration
COPY redis.conf /etc/redis/redis.conf

# Copy OpenSearch configuration
COPY opensearch.yml /usr/share/opensearch/config/opensearch.yml

# Copy OpenSearch Dashboards configuration
COPY opensearch_dashboards.yml /usr/share/opensearch-dashboards/config/opensearch_dashboards.yml

# Set up RabbitMQ user
RUN rabbitmqctl add_user admin R6zntmydkm2? && \
    rabbitmqctl set_user_tags admin administrator && \
    rabbitmqctl set_permissions -p / admin ".*" ".*" ".*"

# Expose necessary ports
EXPOSE 6379 5672 15672 9200 9600 5601 8000

# Start supervisord
CMD ["/usr/bin/supervisord", "-c", "/etc/supervisor/conf.d/supervisord.conf"]