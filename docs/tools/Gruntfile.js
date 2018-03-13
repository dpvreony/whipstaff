module.exports = function (grunt) {
    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        banner: '/* <%= pkg.name %> version: <%= pkg.version %> \n * Copyright (c) <%= pkg.author %> <%= grunt.template.today("yyyy") %> */\n',
        concat: {
            options: {
                banner: '<%= banner %>',
                stripBanners: true
            },
            dist: {
                files: {
                    '../css/<%= pkg.name %>.site-<%= pkg.version %>.css': [
                        '../css/main.css',
                        '../css/bootstrap.css'
                    ],
                    '../scripts/<%= pkg.name %>.core-<%= pkg.version %>.js': [
                        '../scripts/google-analytics.js',
                        '../scripts/jquery-1.11.1.js',
                        '../scripts/bootstrap.js',
                        '../scripts/pretty.js',
                        '../scripts/site.js'
                    ],
                }
            },
        },
        uglify: {
            options: {
                banner: '<%= banner %>',
                stripBanners: true,
                sourceMap: true
            },
            dist: {
                files: {
                    '../scripts/<%= pkg.name %>.core-<%= pkg.version %>.min.js': '../scripts/<%= pkg.name %>.core-<%= pkg.version %>.js'
                }
            }
        },
        cssmin: {
            options: {
                banner: '<%= banner %>',
                stripBanners: true
            },
            dist: {
                files: {
                    '../css/<%= pkg.name %>.site-<%= pkg.version %>.min.css': '../css/<%= pkg.name %>.site-<%= pkg.version %>.css'
                }
            }
        },
        shell: {
            jekyllBuild: {
                command: 'jekyll build --source ../ --destination ../_site'
            }
        },
        watch: {
            files: [
                '../_includes/*.html',
                '../_docs/*.md',
                '../scripts/site.js',
                '../_press/*.md',
                '../_blog/*.md',
                '../_layouts/*.html',
                '../_posts/*.md',
                '../css/main.css',
                '../_config.yml',
                '../index.html'
            ],
            tasks: ['concat', 'uglify', 'cssmin', 'shell:jekyllBuild'],
            options: {
                interrupt: true,
                atBegin: true
            }
        }
    });

    // Load the plugin that provides the "uglify" task.
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-css');
    grunt.loadNpmTasks('grunt-shell');
    grunt.loadNpmTasks('grunt-contrib-watch');

    // Default task(s).
    grunt.registerTask('default', ['watch']);
};