# Security Decisions Log

## Angular Version Update (April 2026)

**Date**: 2026-04-14

**Decision**: Upgrade from Angular 18 to Angular 19.2.20+

**Reason**: 
Angular 18.2.14 contains multiple critical security vulnerabilities with NO patches available:

### Vulnerabilities Identified
1. **XSRF Token Leakage** (CVE affecting @angular/common)
   - Protocol-relative URLs vulnerability
   - Affects: Angular 17.0.0 - 18.2.14
   - Patch: Only available in 19.2.16+, 20.3.14+, 21.0.1+

2. **XSS in i18n attribute bindings** (CVE affecting @angular/compiler, @angular/core)
   - Multiple XSS vulnerabilities
   - Affects: Angular 17.0.0 - 18.2.14
   - Patch: Not available for 18.x

3. **XSS via Unsanitized SVG Script Attributes** 
   - Affects: Angular <= 18.2.14
   - Patch: Not available for 18.x

4. **Stored XSS via SVG Animation, SVG URL and MathML Attributes**
   - Affects: Angular <= 18.2.14  
   - Patch: Not available for 18.x

5. **Angular i18n XSS vulnerability**
   - Affects: Angular <= 18.2.14
   - Patch: Not available for 18.x

### Patched Versions Available
- ✅ Angular 19.2.20+
- ✅ Angular 20.3.18+
- ✅ Angular 21.2.4+

### Decision Rationale
1. **Security First**: Even for demo applications, showcasing vulnerable code sets a bad precedent
2. **Best Practices**: Demonstrates using latest stable, patched versions
3. **Minimal Impact**: Angular 19 maintains same architecture (standalone components work identically)
4. **Early Stage**: Frontend scaffold just created, easy to regenerate with correct version

### Action Taken
- Updated Guidelines to specify Angular 19.2.20+ as minimum version
- Updated README with security note
- Frontend will be regenerated with Angular 19.2.20+
- All documentation updated to reflect secure version

### References
- GitHub Advisory Database
- Angular Security Advisories
- gh-advisory-database tool output

---

**Approved By**: Development Team  
**Impact**: Low (early stage, minimal code)  
**Status**: Implemented
