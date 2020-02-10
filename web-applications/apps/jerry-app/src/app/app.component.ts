import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { filter, map } from 'rxjs/operators';

@Component({
    selector: 'jerry-prefix-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {
    title = 'Jerry';

    public selectedPath(): string {
        return this.router.url;
    }

    constructor(
        private router: Router,
        private titleService: Title,
        private activatedRoute: ActivatedRoute) { }

    ngOnInit() {
        const appTitle = this.titleService.getTitle();
        this.router
            .events.pipe(
                filter(event => event instanceof NavigationEnd),
                map(() => {
                    let child = this.activatedRoute.firstChild;
                    while (child.firstChild) {
                        child = child.firstChild;
                    }
                    if (child.snapshot.data['title']) {
                        return this.title + " - " + child.snapshot.data['title'];
                    }
                    return appTitle;
                })
            ).subscribe((ttl: string) => {
                this.titleService.setTitle(ttl);
            });
    }


    menuSelect(menuPath: string) {
        return this.router.navigateByUrl(menuPath);
    }
}
