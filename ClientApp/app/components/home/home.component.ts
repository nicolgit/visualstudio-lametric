import { Router, ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';


@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

    errorString = "";

    userName = "-";

    accessToken = "";
    isAuthenticated = false;


    constructor(private activatedRoute: ActivatedRoute) { }

    ngOnInit(): void {
        this.activatedRoute.queryParams.subscribe((params: Params) => {

            if (params['Error'] != null)
            {
                this.errorString = params['Error'] + ": " + params["ErrorDescription"];
                console.log(this.errorString);
            } 

            this.userName = this.getCookie("lametric-name") == "" ? "World" : decodeURIComponent(this.getCookie("lametric-name"));
            this.accessToken = this.getCookie("lametric-auth-accesstoken");
            this.isAuthenticated = this.accessToken != "";
        });
    }

    public signin() {
        window.location.href = 'https://app.vssps.visualstudio.com/oauth2/authorize?client_id=CE2BF6DB-465F-45D9-9062-2CAFFFCF12B7&response_type=Assertion&state=/&scope=vso.code&redirect_uri=https://blogs.msdn.com/nicold';
    }

    public signout() {
        this.setCookie("lametric-auth-accesstoken", '', -1);
        this.setCookie("lametric-name", '', -1);

        this.userName = "World";
        this.isAuthenticated = false;
        this.accessToken = "";
    }

    private getCookie(name: string) {
        let ca: Array<string> = document.cookie.split(';');
        let caLen: number = ca.length;
        let cookieName = `${name}=`;
        let c: string;

        for (let i: number = 0; i < caLen; i += 1) {
            c = ca[i].replace(/^\s+/g, '');
            if (c.indexOf(cookieName) == 0) {
                return c.substring(cookieName.length, c.length);
            }
        }
        return '';
    }

    private setCookie(name: string, value: string, expireDays: number, path: string = '') {
        let d: Date = new Date();
        d.setTime(d.getTime() + expireDays * 24 * 60 * 60 * 1000);
        let expires: string = `expires=${d.toUTCString()}`;
        let cpath: string = path ? `; path=${path}` : '';
        document.cookie = `${name}=${value}; ${expires}${cpath}`;
    }

}

