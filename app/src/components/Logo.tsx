import styled from 'styled-components'
import {
  flexbox,
  layout,
  space,
  FlexboxProps,
  LayoutProps,
  SpaceProps,
} from 'styled-system'

const Logo = styled.img<FlexboxProps & LayoutProps & SpaceProps>`
  ${flexbox}
  ${layout}
  ${space}
`

export default Logo
