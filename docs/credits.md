---
title: Credits
layout: default
---

<h1>Credits</h1>

{% for category in site.data.credits %}
<h2>{{ category.name }}</h2>
<h3>Projects</h3>
<ul>
    {% for item in category.projects %}
    <li><a href="{{ item.Uri }}">{{ item.name }}</a></li>
    {% endfor %}
</ul>

<h3>Blog Posts</h3>
<ul>
    {% for item in category.blogposts %}
    <li><a href="{{ item.Uri }}">{{ item.name }}</a></li>
    {% endfor %}
</ul>
{% endfor %}
