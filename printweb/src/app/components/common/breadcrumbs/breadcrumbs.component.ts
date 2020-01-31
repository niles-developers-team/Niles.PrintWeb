import { Component } from "@angular/core";
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MenuItem } from 'src/app/models/menuItem.model';
import { filter } from 'rxjs/operators';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'breadcrumbs',
  templateUrl: './breadcrumbs.component.html',
  styleUrls: ['./breadcrumbs.component.scss']
})
export class BreadcrumbsComponent {
  public breadcrumbs: MenuItem[] = [];

  constructor(private _router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.breadcrumbs = this.createBreadcrumbs(this.activatedRoute.root);
    this._router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => this.breadcrumbs = this.createBreadcrumbs(this.activatedRoute.root));
  }

  private createBreadcrumbs(route: ActivatedRoute, path: string = './', breadcrumbs: MenuItem[] = []): MenuItem[] {
    const children: ActivatedRoute[] = route.children;
    if (children.length === 0)
      return breadcrumbs;

    for (const child of children) {
      const parent = child.snapshot.data.parent;
      if(!parent)
       path += child.snapshot.url.map(segment => segment.path).join('/');

      const text = child.snapshot.data.breadcrumb;
      if (!isNullOrUndefined(text)) {
        breadcrumbs.push({ text, path });
      }

      return this.createBreadcrumbs(child, path, breadcrumbs);
    }
  }
}