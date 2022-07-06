import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';     // Pour constructor(private router:Router)  : fonctionne

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {  

  loginObj: any;
  constructor(private router:Router) {
    this.loginObj = {
      userName:'',
      password:''
    };
  }

  ngOnInit(): void {
  }

  login() {
    if (this.loginObj.userName == 'admin' && this.loginObj.password == '1234') {
        this.router.navigateByUrl('home');
    }
    else {
      alert('wrong information');
    }
  }

}

